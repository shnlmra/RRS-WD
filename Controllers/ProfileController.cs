using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Profile()  // Changed return type to IActionResult
        {
            return View("~/Views/Customer/Profile.cshtml");  // Correctly return a view
        }
    }
}
