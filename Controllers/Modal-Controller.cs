using System.Diagnostics;
using RRS.Models;
using Microsoft.AspNetCore.Mvc;

namespace RRS.Controllers
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