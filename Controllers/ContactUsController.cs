using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
