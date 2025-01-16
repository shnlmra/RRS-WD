using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
