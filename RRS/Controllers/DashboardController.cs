using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace YourApp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Sample data for the top stats
            ViewBag.NewReservations = 12;
            ViewBag.AvailableSlots = 7;
            ViewBag.TotalVisitors = 309;
            ViewBag.TotalTransactions = 126;

            // Sample data for the "Number of Reservations" chart
            ViewBag.ReservationsByDay = new Dictionary<string, int>
            {
                { "Monday", 69 },
                { "Tuesday", 17 },
                { "Wednesday", 32 },
                { "Thursday", 20 },
                { "Friday", 41 },
                { "Saturday", 11 },
                { "Sunday", 80 }
            };

            // Sample data for the "Reservation Status Overview" chart
            ViewBag.ReservationStatus = new Dictionary<string, int>
            {
                { "Confirmed", 76 },
                { "Pending", 20 },
                { "Cancelled", 4 }
            };

            // Sample data for the "Total Sales" chart
            ViewBag.TotalSales = new List<int> { 20, 45, 60, 50, 80, 70, 90, 100, 85, 95, 120, 110 };

            return View();
        }
    }
}
