using Microsoft.AspNetCore.Mvc;
using RRS.Models;

namespace RRS.Controllers
{
    public class TableController : Controller
    {
        private static List<Table> _tables = new List<Table>
        {
            new Table { Id = 1, TableNumber = "1", SeatingCapacity = 4, Location = "Indoor", Status = "Available" },
            new Table { Id = 2, TableNumber = "2", SeatingCapacity = 2, Location = "Outdoor", Status = "Reserved" },
            new Table { Id = 3, TableNumber = "3", SeatingCapacity = 6, Location = "Window", Status = "Seated" },
            new Table { Id = 4, TableNumber = "4", SeatingCapacity = 8, Location = "Indoor", Status = "Available" },
            new Table { Id = 5, TableNumber = "5", SeatingCapacity = 4, Location = "Indoor", Status = "Available" },
            new Table { Id = 6, TableNumber = "6", SeatingCapacity = 2, Location = "Outdoor", Status = "Available" },
            new Table { Id = 7, TableNumber = "7", SeatingCapacity = 6, Location = "Window", Status = "Available" },
            new Table { Id = 8, TableNumber = "8", SeatingCapacity = 8, Location = "Indoor", Status = "Available" },
            new Table { Id = 9, TableNumber = "9", SeatingCapacity = 6, Location = "Window", Status = "Available" },
            new Table { Id = 10, TableNumber = "10", SeatingCapacity = 8, Location = "Indoor", Status = "Available" }
        };

        public static List<Table> GetTables()
        {
            return _tables;
        }

        public IActionResult Index()
        {
            return View(_tables);
        }

        public IActionResult AddTable()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTable(Table table)
        {
            if (_tables.Any(t => t.TableNumber == table.TableNumber))
            {
                ModelState.AddModelError("TableNumber", "Table number must be unique.");
                return View(table);
            }

            if (table.SeatingCapacity > 20)
            {
                ModelState.AddModelError("SeatingCapacity", "Seating capacity cannot exceed 20.");
                return View(table);
            }

            table.Id = _tables.Count + 1;
            table.Status = "Available"; // Default status
            _tables.Add(table);
            TempData["SuccessMessage"] = "Table added successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult EditTable(int id)
        {
            var table = _tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                return NotFound(); // Handle case where table does not exist
            }

            return View(table); // Pass the table model to the EditTable view
        }


        public IActionResult DeleteTable(int id)
        {
            var table = _tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                return NotFound();
            }

            _tables.Remove(table);
            TempData["SuccessMessage"] = "Table deleted successfully!";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult EditTable(Table updatedTable)
        {
            var table = _tables.FirstOrDefault(t => t.Id == updatedTable.Id);
            if (table == null)
            {
                return NotFound();
            }

            // Validate unique table number
            if (_tables.Any(t => t.TableNumber == updatedTable.TableNumber && t.Id != updatedTable.Id))
            {
                ModelState.AddModelError("TableNumber", "Table number must be unique.");
            }

            // Validate seating capacity
            if (updatedTable.SeatingCapacity < 1 || updatedTable.SeatingCapacity > 20)
            {
                ModelState.AddModelError("SeatingCapacity", "Seating capacity must be between 1 and 20.");
            }

            if (!ModelState.IsValid)
            {
                return View(updatedTable);
            }

            table.TableNumber = updatedTable.TableNumber;
            table.SeatingCapacity = updatedTable.SeatingCapacity;
            table.Location = updatedTable.Location;
            table.AdditionalNotes = updatedTable.AdditionalNotes;
            table.Status = updatedTable.Status;

            TempData["SuccessMessage"] = "Table updated successfully!";
            return RedirectToAction("Index");
        }

    }
}