using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult StaffAccount()
        {
            return View("~/Views/Home/StaffAccount.cshtml");
        }
    }
}
