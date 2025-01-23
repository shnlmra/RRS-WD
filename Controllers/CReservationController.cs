using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class CReservationController : Controller
    {
        public IActionResult CReservation()
        {
            return View("~/Views/Reservation/CReservation.cshtml");

        }
        
        public IActionResult Reservation()
        {
            return View();
        }
    }
}
