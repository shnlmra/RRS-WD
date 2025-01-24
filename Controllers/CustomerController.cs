using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Customers()
        {
            return View("~/Views/Customer/Customers.cshtml");
        }

    }
}
