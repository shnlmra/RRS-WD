using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Calendar()
        {
            return PartialView("~/Views/Reservation/Calendar.cshtml");
        }
    }
}
