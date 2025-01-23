using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class CustomerLoginController : Controller
    {
        public IActionResult CustomerLogin()
        {
            return View("~/Views/Account/CustomerLogin.cshtml");
        }


    }
}
