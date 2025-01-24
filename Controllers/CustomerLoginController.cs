using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace RRS.Controllers
{
    public class CustomerLoginController : Controller
    {
        // Set the constant for cookie authentication scheme
        private const string CookieAuthenticationDefaults = "Cookies";

        // Returns the CustomerLogin view
        public IActionResult CustomerLogin()
        {
            return View("~/Views/Account/CustomerLogin.cshtml");
        }
    }
}
