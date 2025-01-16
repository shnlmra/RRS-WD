using Microsoft.AspNetCore.Mvc;
using RRS.Data;
using RRS.Models;

namespace RRS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var categoriesWithProductCount = context.Categories
                .Select(category => new CategoryViewModel
                {
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    ItemsCount = context.Menus.Count(menu => menu.CategoryId == category.Id) // Assuming Menu has a CategoryId property
                })
                .ToList();

            return View(categoriesWithProductCount);
        }
    }
}
