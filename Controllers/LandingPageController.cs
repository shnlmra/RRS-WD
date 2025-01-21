using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class LandingPageController : Controller
    {
        public IActionResult LandingPage()
        {
            return View();
        }
    }
}
