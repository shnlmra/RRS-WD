using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Data;
using RRS.Models;
using System.Diagnostics;

namespace RRS.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly ApplicationDbContext context;

        public ReservationController(ILogger<ReservationController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }


        public IActionResult Index()
        {
            List<Reservation> reservations = context.Reservations
                .Include(r => r.Menu)
                .Include(r => r.Table)
                .Include(r => r.Customer)
                .ToList();

            return View(reservations);
        }

        //public IActionResult ViewDetails(int id)
        //{
        //    Reservation reservation = context.Reservations.FirstOrDefault(r => r.Id == id);

        //    if (reservation == null)
        //    {
        //        TempData["ErrorMessage"] = "Reservation not found!";
        //        return RedirectToAction("Index");
        //    }

        //    return PartialView("ReservationDetails", reservation);
        //}

        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}