using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class MyReservationController : Controller
    {
        public IActionResult MyReservation()
        {
            return View("~/Views/Customer/MyReservation.cshtml");
        }
    }
}
