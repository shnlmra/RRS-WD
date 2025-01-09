using Menu_Management.Data; // DbContext reference
using Menu_Management.Models; // Model Reference
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Menu_Management.Controllers
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
            var menuItems = _context.Add_Menu_Modal.ToList();

            // Pass the data to the view using ViewData or ViewModel
            ViewData["Title"] = "Main Page";
            return View("~/Views/Menu-Management/MainPage.cshtml", menuItems);
        }

        // Handle POST Request to Add a Menu Item
        [HttpPost]
        public IActionResult AddMenuItem(Add_Menu_Modal menu)
        {
            if (ModelState.IsValid)
            {
                // Add the new menu item to the database
                _context.Add_Menu_Modal.Add(menu);
                _context.SaveChanges();

                // Redirect back to the MainPage after adding
                return RedirectToAction("MainPage");
            }

            // Return the view with validation errors if the model is invalid
            return View("MainPage");
        }
    }
}
