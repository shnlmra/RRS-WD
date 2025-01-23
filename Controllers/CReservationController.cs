using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class CReservationController : Controller
    {
        public IActionResult Reservation()
        {
            return View("~/Views/Customer/Reservation.cshtml");
        }
    }
}
