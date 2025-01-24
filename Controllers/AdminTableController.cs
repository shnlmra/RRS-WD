using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class AdminTableController : Controller
    {
        public IActionResult AdminTable()
        {
            return View("~/Views/Admin/TableAdmin.cshtml");
        }
    }
}
