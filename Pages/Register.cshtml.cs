using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FSD08_AppDev2Project.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly ILogger<RegisterModel> logger;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public IFormFile FileInput { get; set; }

        public List<SelectListItem> roles { get; set; } = new List<SelectListItem>
            {
                new SelectListItem { Value = "Admin", Text = "Admin" },
                new SelectListItem { Value = "HiringManager", Text = "HiringManager" },
                new SelectListItem { Value = "Applicant", Text = "Applicant"  },
            };

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            public ApplicationRoles role { get; set; }
            [Required(ErrorMessage = "The UserName field is required.")]
            [Display(Name = "Username")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "The Email field is required.")]
            [EmailAddress(ErrorMessage = "Invalid Email Address.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "The Password field is required.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "The Confirm Password field is required.")]
            [Display(Name = "Confirm Password")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "The Country field is required.")]
            [Display(Name = "Country")]
            public string Country { get; set; }

            [Required(ErrorMessage = "The State field is required.")]
            [Display(Name = "State")]
            public string State { get; set; }

            [Required(ErrorMessage = "The City field is required.")]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required(ErrorMessage = "The Zipcode field is required.")]
            [Display(Name = "Zipcode")]
            public string ZipCode { get; set; }

            [Required(ErrorMessage = "The Phone number field is required.")]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            //ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // var user = new ApplicationUser {UserName = "miral1", Email = "miral@sample.com", PhoneNumber = "123-456-4444", City ="Montreal", Country="Canada", State ="Quebec", Active = true, Zipcode = "P4W 2A5" };
                // var result = await userManager.CreateAsync(user, "Test@2023");

                var user = new ApplicationUser
                {
                    UserName = Input.UserName,
                    Email = Input.Email,
                    Country = Input.Country,
                    State = Input.State,
                    City = Input.City,
                    Zipcode = Input.ZipCode,
                    PhoneNumber = Input.PhoneNumber,
                    Active = true
                };
                var result = await userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    //user added, so add image
                    if (FileInput != null && FileInput.Length > 0)
                    {
                        string fileName = Path.GetFileName(FileInput.FileName);
                        string filePath = Path.Combine("wwwroot", "uploads", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await FileInput.CopyToAsync(stream);
                        }
                        AzureBlobUtil.UploadIconToBlob(filePath, user.Id);
                        System.IO.File.Delete(filePath);
                    }
                    //Add image code ends

                    //var result2 = await userManager.AddToRoleAsync(user, role.ToString());
                    var result2 = await userManager.AddToRoleAsync(user, Input.role.Name);
                    if (result2.Succeeded)
                    {
                        logger.LogInformation($"User {Input.Email} create a new account with password");
                        return RedirectToPage("RegisterSuccess", new { email = Input.Email });
                    }
                    else
                    {

                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }

    }
}
