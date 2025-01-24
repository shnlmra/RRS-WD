using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Data;
using RRS.Models;

namespace RRS.Controllers
{
    public class DashboardController : Controller
    {
		private readonly ApplicationDbContext _context;

		public DashboardController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var today = DateOnly.FromDateTime(DateTime.Now);

			var upcomingReservations = await _context.Reservations
				.CountAsync(r => r.ReservationDate >= today && r.Status == "Pending");

			var availableTables = await _context.Tables
				.CountAsync(t => t.Status == "available");

			var finishedReservations = await _context.Reservations
				.CountAsync(r => r.Status == "Completed");

			//var seatedGuests = await _context.Reservations
			//	.Where(r => r.Status == "Seated")
			//	.SumAsync(r => r.Seats);

			var totalTables = await _context.Tables.CountAsync();

			// Pass data to the view
			var dashboardData = new DashboardViewModel
			{
				UpcomingReservations = upcomingReservations,
				AvailableTables = availableTables,
				FinishedReservations = finishedReservations,
				//SeatedGuests = seatedGuests,
				TotalTables = totalTables
			};

			return View(dashboardData);
		}
    }
}
