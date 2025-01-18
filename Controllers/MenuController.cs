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
            List<Menu> menu = context.Menus.Where(m => m.IsDeleted == false).ToList();

            return View(menu);
        }

        public IActionResult Create()
        {
            Menu menu = new Menu();

            return PartialView("AddMenuModal", menu);
        }


        [HttpPost]
        public IActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                menu.CreatedAt = DateTime.Now;
                menu.UpdatedAt = DateTime.Now;

                context.Menus.Add(menu);
                context.SaveChanges();

                TempData["SuccessMessage"] = "Menu item added successfully!";

                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Failed to add menu item.";
            return RedirectToAction("Index");
        }

        
        public IActionResult Edit(int id)
        {
            Menu menuItem = context.Menus.Where(m => m.Id == id).FirstOrDefault();

            return PartialView("EditMenuModal", menuItem);
        }

        [HttpPost]
        public IActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                // Find the existing menu item
                Menu existingMenu = context.Menus.FirstOrDefault(m => m.Id == menu.Id);

                if (existingMenu != null)
                {
                    if (menu.Name == existingMenu.Name && menu.Description == existingMenu.Description 
                        && menu.Price == existingMenu.Price && menu.Category == existingMenu.Category)
                    {
                        TempData["InformationMessage"] = "No changes have been made.";

                        return RedirectToAction("Index");
                    }


                    existingMenu.Name = menu.Name;
                    existingMenu.Description = menu.Description;
                    existingMenu.Price = menu.Price;
                    existingMenu.Category = menu.Category;
                    existingMenu.UpdatedAt = DateTime.Now;

                    context.SaveChanges();

                    // Return the updated menu object in the response
                    //return Json(new { success = true, updatedMenu = existingMenu });
                    TempData["SuccessMessage"] = "Menu details updated successfully!";

                    return RedirectToAction("Index");
                }
            }

            // If the model is invalid or the menu doesn't exist, return failure
            //return Json(new { success = false, error = "Invalid menu data or menu not found." });

            TempData["ErrorMessage"] = "Invalid menu data or menu not found.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Menu menuToDelete = context.Menus.Where(m => m.Id == id).FirstOrDefault();

            return PartialView("DeleteMenuModal", menuToDelete);
        }

        [HttpPost]
        public IActionResult Delete(Menu menuToDelete)
        {

            Menu existingMenu = context.Menus.FirstOrDefault(m => m.Id == menuToDelete.Id);

            if ( existingMenu != null )
            {
                existingMenu.IsDeleted = true;
                existingMenu.UpdatedAt = DateTime.Now;

                context.SaveChanges();

                TempData["SuccessMessage"] = "Menu item deleted successfully";
                return RedirectToAction("Index");
            }
            
            TempData["ErrorMessage"] = "Invalid menu data or menu not found.";
            return RedirectToAction("Index");


           
        }

    }
}
