using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminPayments()
        {
            return View("AdminPayments");
        }

        public IActionResult PaymentStaff()
        {
            return View("PaymentStaff");
        }
    }
}
