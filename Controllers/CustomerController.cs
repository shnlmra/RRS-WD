using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Customer()
        {
            return View("~/Views/Home/Customer.cshtml");
        }
    }
}
