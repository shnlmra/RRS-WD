using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class FormController : Controller
    {
        public IActionResult Form()
        {
            return View();
        }
    }
}
