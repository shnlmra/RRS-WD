using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class AdminProfileController : Controller
    {
        public IActionResult AdminProfile()
        {
            return View("~/Views/Admin/AdminProfile.cshtml");
        }
    }
}
