using Microsoft.AspNetCore.Mvc;

namespace Menu_Management.Controllers
{
    public class MainPageController : Controller
    {
        public IActionResult MainPage()
        {
            // Explicitly reference the path where MainPage.cshtml is located
            ViewData["Title"] = "Main Page";
            return View("~/Views/Menu-Management/MainPage.cshtml");
        }
    }
}
