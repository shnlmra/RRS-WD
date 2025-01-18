using Microsoft.AspNetCore.Mvc;
using RRS.Data;
using RRS.Models;
using RRS.Models.DataTransferObject;
using RRS.Models.Entities;

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

            var viewModel = new MenuDto
            {
                Categories = categories,
                Menus = menu
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Add(MenuDto menuDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Reload categories for the dropdown if the model is invalid
                    menuDto.Categories = context.Categories.ToList();
                    return RedirectToAction("Index", menuDto);
                }

                // Create a new Menu entity from the DTO
                var menu = new Menu
                {
                    Name = menuDto.Name,
                    Description = menuDto.Description,
                    Price = menuDto.Price,
                    CategoryId = menuDto.CategoryId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Save the menu to the database
                context.Menus.Add(menu);
                context.SaveChanges();

                // Redirect to Index after successful addition
                TempData["SuccessMessage"] = "Menu item added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging library in a real application)
                Console.WriteLine(ex.Message);

                // Reload categories and display an error message
                menuDto.Categories = context.Categories.ToList();
                TempData["ErrorMessage"] = "An error occurred while adding the menu item.";
                return RedirectToAction("Index", menuDto);
            }
        }

    }
}
