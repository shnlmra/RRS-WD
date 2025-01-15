using Microsoft.AspNetCore.Mvc;
using RRS.Data;

namespace RRS.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext context;

        public MenuController(ApplicationDbContext context)
        {
            this.context = context;
        }


        public IActionResult Index()
        {
            var menu = context.Menus.ToList();
            return View(menu);
        }
    }
}
