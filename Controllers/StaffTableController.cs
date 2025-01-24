using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class StaffTableController : Controller
    {
        public IActionResult StaffTable()
        {
            return View("~/Views/Admin/StaffTable.cshtml");
        }
    }
}
