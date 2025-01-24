using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class PaymentStaffController : Controller
    {
        public IActionResult PaymentStaff()
        {
            return View("~/Views/Admin/PaymentStaff.cshtml");
        }
    }
}
