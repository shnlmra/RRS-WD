using Microsoft.AspNetCore.Mvc;
using RRS.Models;
using System.Collections.Generic;

namespace RRS.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles/UserRole
        public IActionResult UserRole()
        {
            // Mock data - Replace this with data from your database or service
            var userRoles = new List<UserRoleViewModel>
            {
                new UserRoleViewModel { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Role = "Admin", Status = "Active" },
                new UserRoleViewModel { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Role = "Editor", Status = "Active" },
                new UserRoleViewModel { FirstName = "Emily", LastName = "Johnson", Email = "emily.johnson@example.com", Role = "Viewer", Status = "Inactive" },
                new UserRoleViewModel { FirstName = "Michael", LastName = "Brown", Email = "michael.brown@example.com", Role = "Contributor", Status = "Active" }
            };

            return View("~/Views/UserRoles/UserRole.cshtml", userRoles);
        }
    }
}