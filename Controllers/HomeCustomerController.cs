using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class HomeCustomerController : Controller
    {
        public IActionResult HomeCustomer()
        {
            return View("~/Views/Customer/HomeCustomer.cshtml");
        }
    }
}
