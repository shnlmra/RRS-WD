using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Customers()
        {
            return View("~/Views/Customer/HomeCustomer.cshtml");
        }

        public IActionResult Layout()
        {
            return View("~/Views/Shared/_CustomerLayout.cshtml");
        }
    }
}
