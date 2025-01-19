using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult AboutUs()
        {
            return View("~/Views/Home/AboutUs.cshtml");
        }
    }
}
