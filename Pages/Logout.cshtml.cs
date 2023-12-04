using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using FSD08_AppDev2Project.Models; 

namespace FSD08_AppDev2Project.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<RegisterModel> logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<RegisterModel> logger) {
            this.signInManager = signInManager;
            this.logger = logger;
        }
        public IActionResult OnGetAsync()
        {
            signInManager.SignOutAsync();
            TempData["AlertMessage"] = "Logout successful!";
            if (signInManager.IsSignedIn(User)) {
                logger.LogInformation($"User {User.Identity.Name} logged out");
            }
            return RedirectToPage("Login");
        }
    }
}
