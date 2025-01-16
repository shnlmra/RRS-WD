using Microsoft.AspNetCore.Mvc;
using RRS.Data;
using RRS.Models;

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
            var categories = context.Categories.ToList();
            var menu = context.Menus.ToList();

            var viewModel = new MenuViewModel
            {
                Categories = categories,
                Menus = menu
            };

            return View(viewModel);
        }
    }
}
