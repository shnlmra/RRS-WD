using Microsoft.AspNetCore.Mvc;
using RRS.Models;
using System.Diagnostics;

namespace RRS.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ILogger<ReservationController> _logger;
        private static List<Table> _tables = TableController.GetTables(); // Get the latest table list

        public ReservationController(ILogger<ReservationController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var reservations = new List<Reservation>
            {
                new Reservation { Time = "7:00 PM", Name = "Miyuki Mharie Parocha", Guests = 3, Table = 32, Status = "Upcoming" },
                new Reservation { Time = "8:00 PM", Name = "Shanella Cagulang", Guests = 6, Table = 7, Status = "Upcoming" },
                new Reservation { Time = "5:30 PM", Name = "Darben Lamonte", Guests = 7, Table = 19, Status = "Seated" },
            };

            // Pass the table list to the view
            ViewBag.Tables = _tables;

            return View(reservations);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}