using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class AuditLogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
