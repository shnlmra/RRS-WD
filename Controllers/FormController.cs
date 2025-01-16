using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class FormController : Controller
    {
        public IActionResult Form()
        {
            return View("~/Views/Reservation/Form.cshtml");
        }
    }
}
