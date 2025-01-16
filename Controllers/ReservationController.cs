using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
