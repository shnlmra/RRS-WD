using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class ContacUsController : Controller
    {
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
