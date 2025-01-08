using System.Diagnostics;
using Menu_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Menu_Management.Controllers
{
    public class MenuManagementController : Controller
    {
        public IActionResult AddMenuModal()
        {
            return View();
        }

        public IActionResult DeleteMenuModal()
        {
            return View();
        }
    }
}