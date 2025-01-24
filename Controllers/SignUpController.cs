using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
	public class SignUpController : Controller
	{
		public IActionResult SignUp()
		{
			return View("~/Views/Account/Signup.cshtml");
		}
	}
}