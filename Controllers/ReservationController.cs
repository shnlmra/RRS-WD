using Microsoft.AspNetCore.Mvc;
using RRS.Data;
using RRS.Models.Entities;
using RRS.Services;

namespace RRS.Controllers
{
	public class ReservationController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IEmailService _emailService;
		private readonly ILogger<ReservationController> _logger;

		public ReservationController(ApplicationDbContext context, IEmailService emailService, ILogger<ReservationController> logger)
		{
			_context = context;
			_emailService = emailService;
			_logger = logger;
		}

		public IActionResult Index()
		{
			var reservations = _context.Reservations.ToList();
			return View(reservations);
		}

		// Create a reservation
		// most likely, will be placed on a controller where most applicable
		[HttpPost]
		public IActionResult Create(Reservation reservation)
		{
			if (ModelState.IsValid)
			{
				reservation.Status = "Pending";
				_context.Reservations.Add(reservation);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(reservation);
		}

		// EMAIL SENDING // AADMIN CONFIRM DONE LOGS
		[HttpPost]
		public async Task<IActionResult> Confirm(int id)
		{
			var reservation = _context.Reservations.Find(id);
			if (reservation == null) return NotFound();

			// Update the status of the reservation
			reservation.Status = "Confirmed";
			_context.SaveChanges();

			// Log action for modification
			var log = new ActionLog
			{
				ReservationId = reservation.Id,
				Action = "Modify",
				ActionDate = DateTime.Now,
				Notes = "Reservation modified by admin",
				PerformedBy = "Admin",  // Log who performed the action
				AdminUser = "admin user", // The admin performing the action
				CustomerEmail = null, // No customer email when an admin performs the action
			};
			_context.ActionLogs.Add(log);
			_context.SaveChanges();

			// Construct the email body with the custom HTML template
			string subject = "Reservation Confirmed";
			string body = $@"
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <title>RRS Confirmation Email</title>
</head>	
<body style=""font-family: 'Poppins', Arial, sans-serif; background-color: #f5f5f5; margin: 0; padding: 0; text-align: center;"">
    <table align=""center"" style=""background-color: white; border-radius: 10px; box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1); padding: 20px 30px; width: 400px; margin-top: 50px;"">
        <tr>
            <td colspan=""2"" style=""font-size: 20px; font-weight: bold; padding-bottom: 20px; text-align: center;"">
                Reservation Confirmation
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: left; padding: 10px 0;"">
                Hi {reservation.FirstName},
            </td>
        </tr>
                <tr>
          	<td colspan=""2"" style=""text-align: left; padding: 10px 0;"">
                Thank you for reserving your schedule with us.
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""padding-top: 30px; padding-bottom: 10px; text-align: left; font-weight: bold;"">
                Reservation Summary
            </td>
        </tr>
        <tr>
            <td style=""padding: 5px 10px; text-align: left;"">Date of Reservation</td>
            <td style=""padding: 5px 10px; text-align: right;"">{reservation.ReservationDate.ToShortDateString()}</td>
        </tr>
        <tr>
            <td style=""padding: 5px 10px; text-align: left;"">Time Slot</td>
            <td style=""padding: 5px 10px; text-align: right;"">{reservation.ReservationTime}</td>
        </tr>
        <tr>
            <td style=""padding: 5px 10px; text-align: left;"">Number of Guests</td>
            <td style=""padding: 5px 10px; text-align: right;"">{reservation.Seats}</td>
        </tr>
        <tr>
            <td style=""padding: 5px 10px; text-align: left;"">Table Number</td>
            <td style=""padding: 5px 10px; text-align: right;"">{reservation.Seats}</td>
        </tr>
        <tr>
            <td style=""padding: 5px 10px; text-align: left;"">Restaurant Branch</td>
            <td style=""padding: 5px 10px; text-align: right;"">SM Fairview</td>
        </tr>
        <tr>
            <td colspan=""2"" style=""padding-top: 60px; text-align: center;"">
                <table width=""100%"">
                    <tr>
                        <td style=""text-align: left;"">
                            <a href=""https://localhost:7189/Reservation/CancelEmail?id={reservation.Id}"" style=""padding: 10px 20px; border: none; border-radius: 5px; font-size: 16px; cursor: pointer; background-color: #BB271A; color: white; text-decoration: none; display: inline-block;"">
                                Cancel
                            </a>
                        </td>
                        <td style=""text-align: right;"">
                            <a href=""https://localhost:7189/Reservation/Modify?id={reservation.Id}"" style=""padding: 10px 20px; border: none; border-radius: 5px; font-size: 16px; cursor: pointer; background-color: white; color: #00754A; border: 2px solid #00754A; text-decoration: none; display: inline-block;"">
                                Modify
                            </a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";

			// Send the email
			await _emailService.SendEmailAsync(reservation.EmailAddress, subject, body);

			return RedirectToAction("Index");
		}

		// CANCEL ADMIN DONE LOGS
		[HttpPost]
		public async Task<IActionResult> Cancel(int id)
		{
			var reservation = _context.Reservations.Find(id);
			if (reservation == null) return NotFound();

			// Update the status of the reservation
			reservation.Status = "Cancelled";
			_context.SaveChanges();

			// Log action for modification
			var log = new ActionLog
			{
				ReservationId = reservation.Id,
				Action = "Cancel",
				ActionDate = DateTime.Now,
				Notes = "Reservation cancelled by admin",
				PerformedBy = "Admin",  // Log who performed the action
				AdminUser = "admin user", // The admin performing the action
				CustomerEmail = null, // No customer email when an admin performs the action
			};
			_context.ActionLogs.Add(log);
			_context.SaveChanges();

			// Send cancellation email
			string subject = "Reservation Cancelled";
			string body = $@"
        <h1>Your Reservation has been Cancelled</h1>
        <p>Dear {reservation.FirstName},</p>
        <p>Your reservation for {reservation.ReservationDate.ToShortDateString()} at {reservation.ReservationTime} has been cancelled.</p>
        <p>If this was a mistake, please contact us to rebook your reservation.</p>";

			await _emailService.SendEmailAsync(reservation.EmailAddress, subject, body);

			return RedirectToAction("Index");
		}

		// GET: Reservation/Modify // CUSTOMER FORM
		public IActionResult Modify(int id)
		{
			var reservation = _context.Reservations.Find(id);
			if (reservation == null) return NotFound();

			// Pre-fill the form with the reservation data
			return View(reservation);
		}

		[HttpPost] // -- CUSTOMER DONE  LOGS
		public IActionResult Modify(int id, Reservation modifiedReservation)
		{
			_logger.LogInformation("Attempting to modify reservation with ID {ReservationId}.", id);

			if (id != modifiedReservation.Id)
			{
				_logger.LogWarning("ID mismatch. Expected {ExpectedId}, but received {ReceivedId}.", id, modifiedReservation.Id);
				return NotFound();  // Ensure the reservation IDs match
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(modifiedReservation); // Update the reservation
					_context.SaveChanges(); // Save the changes to the database
					_logger.LogInformation("Reservation {ReservationId} modified successfully.", modifiedReservation.Id);

					// Log action for modification
					var log = new ActionLog
					{
						ReservationId = modifiedReservation.Id,
						Action = "Modify",
						ActionDate = DateTime.Now,
						Notes = "Reservation modified by customer",
						PerformedBy = "Customer",  // Log who performed the action
						AdminUser = null, // The admin performing the action
						CustomerEmail = modifiedReservation.EmailAddress, // No customer email when an admin performs the action
					};
					_context.ActionLogs.Add(log);
					_context.SaveChanges();

					return RedirectToAction("Index"); // Redirect to the list or confirmation page
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "An error occurred while saving the reservation with ID {ReservationId}.", modifiedReservation.Id);
					ModelState.AddModelError(string.Empty, "An error occurred while saving the changes.");
				}
			}
			else
			{
				_logger.LogWarning("Model state is invalid for reservation {ReservationId}.", modifiedReservation.Id);

				var errors = ModelState.Values.SelectMany(v => v.Errors);
				foreach (var error in errors)
				{
					_logger.LogWarning($"ModelState Error: {error.ErrorMessage}");
				}
			}

			// Return the form with validation errors (if any)
			return View(modifiedReservation);
		}

		// GET: Reservation/CancelEmail -- CUSTOMER
		public IActionResult CancelEmail(int id)
		{
			var reservation = _context.Reservations.Find(id);
			if (reservation == null) return NotFound();

			return View(reservation); // Show a cancellation confirmation prompt
		}

		// POST: Reservation/Cancel -- CUSTOMER DONE LOGS
		[HttpPost, ActionName("ConfirmCancel")]
		public IActionResult ConfirmCancel(int id)
		{
			var reservation = _context.Reservations.Find(id);
			if (reservation == null) return NotFound();

			reservation.Status = "Cancelled";
			_context.SaveChanges();

			// Log cancellation action
			var log = new ActionLog
			{
				ReservationId = id,
				Action = "Cancel",
				ActionDate = DateTime.Now,
				Notes = "Reservation cancelled by customer",
				PerformedBy = "Customer",  // Log who performed the action
				AdminUser = null, // The admin performing the action
				CustomerEmail = reservation.EmailAddress, // No customer email when admin cancels the reservation
			};
			_context.ActionLogs.Add(log);
			_context.SaveChanges();

			return RedirectToAction("Index"); // Redirect to the reservation list or a confirmation page
		}

	}
}
