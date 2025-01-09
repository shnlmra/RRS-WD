using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MainWebController : Controller
    {
        public IActionResult MainWebLayout()
        {
            Console.WriteLine("Attempting to render view: ~/Views/Web/MainWeb_Layout.cshtml");
            return View("~/Views/Web/MainWeb_Layout.cshtml");
        }
    }
}
