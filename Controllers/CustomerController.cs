using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CustomersReservation()
        {
            return View();
        }
    }
}
