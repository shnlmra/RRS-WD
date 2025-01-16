using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
