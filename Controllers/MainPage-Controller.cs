using RRS.Data; // DbContext reference
using RRS.Models; // Model Reference
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace RRS.Controllers
{
    public class MainPageController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext via Dependency Injection
        public MainPageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Render MainPage with Data
        public IActionResult MainPage()
        {
            // Fetch data from the database
            var menuItems = _context.Menu.ToList();

            // Pass the data to the view using ViewData or ViewModel
            ViewData["Title"] = "Main Page";
            return View("~/Views/Menu-Management/MainPage.cshtml", menuItems);
        }

        // Handle POST Request to Add a Menu Item
        [HttpPost]
        public IActionResult AddMenuItem(Menu menu)
        {
            if (ModelState.IsValid)
            {
                // Add the new menu item to the database
                _context.Menu.Add(menu);
                _context.SaveChanges();

                // Redirect back to the MainPage after adding
                return RedirectToAction("MainPage");
            }

            // Return the view with validation errors if the model is invalid
            return View("MainPage");
        }
    }
}
