using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Data;
using RRS.Models;
using RRS.Services;
using System.Diagnostics;

namespace RRS.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly ApplicationDbContext context;
		private readonly IEmailService _emailService;

		public ReservationController(ApplicationDbContext context, IEmailService emailService, ILogger<ReservationController> logger)
		{
			_logger = logger;
			_emailService = emailService;
			this.context = context;
        }

		// GET: Reservation
		public IActionResult Index()
        {
            List<Reservation> reservations = context.Reservations
                .Include(r => r.Menu)
                .Include(r => r.Table)
                .Include(r => r.Customer)
                .ToList();

            return View(reservations);
        }

		[HttpGet] // Called in Reservation/Index.cshtml for viewing details
		public IActionResult GetReservationDetails(int id)
		{
			var reservation = context.Reservations
				.Include(r => r.Customer)
				.Include(r => r.Table)
				.Include(r => r.Menu)
				.FirstOrDefault(r => r.Id == id);

			if (reservation == null)
			{
				return NotFound();
			}

			var result = new
			{
				customerName = $"{reservation.Customer.FirstName} {reservation.Customer.LastName}",
				reservationDate = reservation.ReservationDate.ToString("MMMM dd, yyyy"),
				reservationTime = reservation.ReservationTime.ToString("hh:mm tt"),
				numberOfGuests = reservation.NumberOfGuest,
				tableNumber = reservation.Table.TableNumber,
				menuName = reservation.Menu.Name,
				occasionType = reservation.OccasionType,
				specialRequest = reservation.SpecialRequest,
				status = reservation.Status
			};

			return Json(result);
		}

		// Error page
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		// Admin Index
		public IActionResult AdminIndex(string status = null)
		{
			List<Reservation> reservations = context.Reservations
				.Include(r => r.Menu)
				.Include(r => r.Table)
				.Include(r => r.Customer)
				.ToList();

			return View(reservations);
		}

		// Confim Manual
		public async Task<IActionResult> Confirm(int id)
		{
			var reservation = context.Reservations
				.Include(r => r.Customer)
				.Include(r => r.Table)
				.Include(r => r.Menu)
				.FirstOrDefault(r => r.Id == id);

			if (reservation == null) return NotFound();

			reservation.Status = "Confirmed";
			context.Reservations.Update(reservation);

			// Log the action
			var log = new ActionLog
			{
				ReservationId = reservation.Id,
				Action = "Confirm",
				ActionDate = DateTime.Now,
				Notes = "Reservation confirmed by admin.",
				PerformedBy = "Admin 1"
			};
			context.ActionLogs.Add(log);

			await context.SaveChangesAsync();

			try
			{
				// Send confirmation email
				await SendConfirmationEmail(reservation);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to send confirmation email for reservation ID: {Id}", reservation.Id);
			}

			return RedirectToAction("Index");
		}

		[HttpPost] // Manual Cancel
		public async Task<IActionResult> Cancel(int id)
		{
			var reservation = await context.Reservations.FindAsync(id);
			if (reservation == null)
			{
				return NotFound();
			}

			reservation.Status = "Cancelled";
			context.Update(reservation);
			await context.SaveChangesAsync();

			return RedirectToAction(nameof(AdminIndex));
		}

		private async Task SendConfirmationEmail(Reservation reservation)
		{
			if (reservation == null || reservation.Customer == null || string.IsNullOrEmpty(reservation.Customer.Email))
			{
				throw new ArgumentNullException(nameof(reservation), "Reservation or Customer information is missing.");
			}

			string emailBody = $"Dear {reservation.Customer.FirstName}, your reservation for {reservation.ReservationDate} at {reservation.ReservationTime} has been confirmed.";
			emailBody += $" Click here to modify: <a href='ModifyLink'>Modify</a> or cancel: <a href='CancelLink'>Cancel</a>.";

			// Use email service
			await _emailService.SendEmailAsync(reservation.Customer.Email, "Reservation Confirmed", emailBody);
		}

		public async Task<IActionResult> Modify(int id)
		{
			var reservation = await context.Reservations.FindAsync(id);
			if (reservation == null) return NotFound();

			return View(reservation);
		}

		[HttpPost]
		public async Task<IActionResult> Modify(Reservation updatedReservation)
		{
			if (ModelState.IsValid)
			{
				var reservation = await context.Reservations.FindAsync(updatedReservation.Id);
				if (reservation == null) return NotFound();

				reservation.ReservationDate = updatedReservation.ReservationDate;
				reservation.ReservationTime = updatedReservation.ReservationTime;
				reservation.NumberOfGuest = updatedReservation.NumberOfGuest;
				reservation.Status = "Confirmed";

				context.Reservations.Update(reservation);

				// Log the action
				var log = new ActionLog
				{
					ReservationId = reservation.Id,
					Action = "Modify",
					ActionDate = DateTime.Now,
					Notes = "Reservation modified by customer.",
					PerformedBy = "Admin 1",
				};
				context.ActionLogs.Add(log);

				await context.SaveChangesAsync();
				return RedirectToAction("AdminIndex");
			}
			return View(updatedReservation);
		}
	}
}