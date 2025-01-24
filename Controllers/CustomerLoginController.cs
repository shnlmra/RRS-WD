using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Linq;

namespace RRS.Controllers
{
    public class CustomerLoginController : Controller
    {
        // Set the constant for cookie authentication scheme
        private const string CookieAuthenticationDefaults = "Cookies";

        // Returns the CustomerLogin view
        public IActionResult CustomerLogin()
        {
            return View("~/Views/Account/CustomerLogin.cshtml");
        }

        // Initiates Google authentication challenge
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse", "CustomerLogin")
                });
        }

        // Handles the response after the user logs in via Google
        public async Task<IActionResult> GoogleResponse()
        {
            // Authenticate using the cookie authentication scheme
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults);

            // Check if authentication was successful
            if (result.Succeeded)
            {
                // Retrieve claims and return as JSON
                var claims = result.Principal?.Identities
                    .FirstOrDefault()?
                    .Claims
                    .Select(claim => new
                    {
                        claim.Issuer,
                        claim.OriginalIssuer,
                        claim.Type,
                        claim.Value
                    });

                return Json(claims);
            }

            // If authentication fails, return unauthorized
            return Unauthorized();
        }
    }
}
