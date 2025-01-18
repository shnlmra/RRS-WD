using Microsoft.AspNetCore.Mvc;
using RRS.Data;
using RRS.Models;

namespace RRS.Controllers
{
    public class TableController : Controller
    {
        private readonly ApplicationDbContext context;

        public TableController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Table> GetTables()
        {
            List<Table> tables = context.Tables.Where(t => !t.IsDeleted).ToList();
            return tables;
        }

        public IActionResult Index()
        {
            List<Table> tables = context.Tables.Where(t => !t.IsDeleted).ToList();

            return View(tables);
        }

        public IActionResult Create()
        {
            Table table = new Table();

            return PartialView("AddTableModal", table);
        }

       [HttpPost]
        public IActionResult Create(Table table)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Set timestamps
                    table.CreatedAt = DateTime.Now;
                    table.UpdatedAt = DateTime.Now;

                    // Add the new table to the database
                    context.Tables.Add(table);
                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Table added successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Capture validation errors
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    TempData["ErrorMessage"] = "Validation errors: " + string.Join(", ", errors);

                    // Return the view with the same model to show validation errors
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;

                // Return the view with the same model to show the error
                return RedirectToAction("Index");
            }
        }


        public IActionResult Edit(int id)
        {
            try
            {
                Table tableToEdit = context.Tables.Where(t => t.Id == id).FirstOrDefault();

                return PartialView("EditTableModal", tableToEdit);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Table not found.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(Table table)
        {
            if (ModelState.IsValid)
            {
                Table existingTable = context.Tables.FirstOrDefault(t => t.Id == table.Id);

                if (existingTable != null)
                {
                    if (table.TableNumber == existingTable.TableNumber && table.SeatingCapacity == existingTable.SeatingCapacity 
                        && table.TableLocation == existingTable.TableLocation && table.Description == existingTable.Description)
                    {
                        TempData["InformationMessage"] = "No changes have been made";
                        return RedirectToAction("Index");
                    }

                    existingTable.TableNumber = table.TableNumber;
                    existingTable.SeatingCapacity = table.SeatingCapacity;
                    existingTable.TableLocation = table.TableLocation;
                    existingTable.Description = table.Description;
                    existingTable.UpdatedAt = DateTime.Now;

                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Table details updated successfuly.";
                    return RedirectToAction("Index");
                }
            }

            TempData["ErrorMessage"] = "Invalid table data or not found.";
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            try
            {
                Table tableToDelete = context.Tables.Where(t => t.Id == id).FirstOrDefault();

                return PartialView("DeleteTableModal", tableToDelete);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Table not found.";
                return RedirectToAction("Index");
            }
        }

        
        [HttpPost]
        public IActionResult Delete(Table table)
        {
            try
            {
                Table existingTable = context.Tables.FirstOrDefault(t => t.Id == table.Id);

                if (existingTable != null)
                {
                    existingTable.IsDeleted = true;
                    existingTable.UpdatedAt = DateTime.Now;

                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Table successfully deleted.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Table not found";
                return RedirectToAction("Index");
            }
        }

        //public IActionResult AddTable()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult AddTable(Table table)
        //{
        //    if (_tables.Any(t => t.TableNumber == table.TableNumber))
        //    {
        //        ModelState.AddModelError("TableNumber", "Table number must be unique.");
        //        return View(table);
        //    }

        //    if (table.SeatingCapacity > 20)
        //    {
        //        ModelState.AddModelError("SeatingCapacity", "Seating capacity cannot exceed 20.");
        //        return View(table);
        //    }

        //    table.Id = _tables.Count + 1;
        //    table.Status = "Available"; // Default status
        //    _tables.Add(table);
        //    TempData["SuccessMessage"] = "Table added successfully!";
        //    return RedirectToAction("Index");
        //}

        //public IActionResult EditTable(int id)
        //{
        //    var table = _tables.FirstOrDefault(t => t.Id == id);
        //    if (table == null)
        //    {
        //        return NotFound(); // Handle case where table does not exist
        //    }

        //    return View(table); // Pass the table model to the EditTable view
        //}


        //public IActionResult DeleteTable(int id)
        //{
        //    var table = _tables.FirstOrDefault(t => t.Id == id);
        //    if (table == null)
        //    {
        //        return NotFound();
        //    }

        //    _tables.Remove(table);
        //    TempData["SuccessMessage"] = "Table deleted successfully!";
        //    return RedirectToAction("Index");
        //}


        //[HttpPost]
        //public IActionResult EditTable(Table updatedTable)
        //{
        //    var table = _tables.FirstOrDefault(t => t.Id == updatedTable.Id);
        //    if (table == null)
        //    {
        //        return NotFound();
        //    }

        //    // Validate unique table number
        //    if (_tables.Any(t => t.TableNumber == updatedTable.TableNumber && t.Id != updatedTable.Id))
        //    {
        //        ModelState.AddModelError("TableNumber", "Table number must be unique.");
        //    }

        //    // Validate seating capacity
        //    if (updatedTable.SeatingCapacity < 1 || updatedTable.SeatingCapacity > 20)
        //    {
        //        ModelState.AddModelError("SeatingCapacity", "Seating capacity must be between 1 and 20.");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(updatedTable);
        //    }

        //    table.TableNumber = updatedTable.TableNumber;
        //    table.SeatingCapacity = updatedTable.SeatingCapacity;
        //    table.Location = updatedTable.Location;
        //    table.AdditionalNotes = updatedTable.AdditionalNotes;
        //    table.Status = updatedTable.Status;

        //    TempData["SuccessMessage"] = "Table updated successfully!";
        //    return RedirectToAction("Index");
        //}

    }
}