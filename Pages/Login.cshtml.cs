using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FSD08_AppDev2Project.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FSD08_AppDev2Project.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ILogger<LoginModel> logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger) {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public class InputModel
        {
            [Required]
            //[EmailAddress]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync() {
            if (ModelState.IsValid)
            {
                
                var result = await signInManager.PasswordSignInAsync(Input.UserName, Input.Password, false, true);
                if (result.Succeeded) {
                    logger.LogInformation($"User {Input.UserName} logged in");
                    return RedirectToPage("LoginSuccess");
                } else {
                    ModelState.AddModelError(string.Empty, "Login failed (user does not exist, password invalid, or account locked out)");
                }
            }
            return Page();
        }
    }
}
