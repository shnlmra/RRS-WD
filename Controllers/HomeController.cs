using Microsoft.AspNetCore.Mvc;
using RRS.Models;
using System.Diagnostics;

namespace RRS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
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
