using Microsoft.AspNetCore.Mvc;
using RRS.Models;

namespace RRS.Controllers
{
    public class AdminProfileController : Controller
    {
        public IActionResult Profile()
        {
            var profile = new AdminProfile
            {
                FirstName = "Miyuki Mharie",
                LastName = "Parocha",
                Email = "miyukimharie@gmail.com",
                PhoneNumber = "09123456789",
                ProfilePicturePath = "/images/default-profile.png"
            };
            return View(profile);
        }

        public IActionResult RestaurantSettings()
        {
            var settings = new RestaurantSettings
            {
                RestaurantName = "Sample Restaurant",
                Address = "123 Food Street, Culinary City",
                PhoneNumber = "09123456789",
                Email = "contact@restaurant.com",
                OperatingHours = "9:00 AM - 10:00 PM",
                Holidays = "Christmas, New Year's Day"
            };
            return View(settings);
        }

        // GET: Change Password
        [HttpGet]
        [Route("AdminProfile/ChangePassword")]
        public IActionResult ChangePassword()
        {
            ViewBag.Message = TempData["SuccessMessage"];
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View();
            }

            if (password.Length < 8)
            {
                ModelState.AddModelError(string.Empty, "Password must be at least 8 characters long.");
                return View();
            }

            // TODO: Add your logic here to update the password in the database.

            // Display success message and redirect to Profile page
            TempData["SuccessMessage"] = "Password has been successfully changed.";
            return RedirectToAction("Profile");

        }

        [HttpGet]
        public IActionResult Notifications()
        {
            var notifications = new List<(string Type, string Message)>
    {
        ("success", "Reservation #1234 has been confirmed."),
        ("error", "Reservation #5678 has been canceled."),
        ("info", "System maintenance scheduled for 12:00 AM."),
        ("warning", "Limited tables available for your selected time."),
        ("neutral", "No new updates at the moment.")
    };

            ViewBag.Notifications = notifications;
            return View();
        }




    }
}
