using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace FSD08_AppDev2Project.Pages
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileModel(AppDev2DbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty]
        public IFormFile FileInput { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public List<AppliedJob> AppliedJobs { get; set; }
        public List<Company> Companies { get; set; }
        [BindProperty]
        public string ProfileImage { get; set; }
        [BindProperty]
        public string UserCV { get; set; }
        [BindProperty]
        public IFormFile FileInputCv { get; set; }

        public string GetCompanyName(int companyId)
        {
            var company = Companies.FirstOrDefault(c => c.Id == companyId);
            return company != null ? company.Name : "Unknown Company";
        }

        public async Task<IActionResult> OnGet()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);

            if (ApplicationUser != null)
            {
                string iconImageURL = AzureBlobUtil.GetBlobUrl(ApplicationUser.Id, "icons", ".jpg");
                string cvURL = AzureBlobUtil.GetBlobUrl(ApplicationUser.Id, "cvs", ".pdf");

                Companies = _db.Companys.ToList();

                ProfileImage = iconImageURL.ToString();
                UserCV = cvURL.ToString();

                // Console.WriteLine("USserCV==================" + UserCV);
                // Console.WriteLine("ProfileImage==================" + ProfileImage);
                // Console.WriteLine("Environment=============================================" + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                AppliedJobs = _db.AppliedJobs
                    .Include(j => j.Job)
                    .Where(ja => ja.Applicant.Id == ApplicationUser.Id)
                    .ToList();
            }

            return Page();
        }

        public class EditProfileInputModel
        {
            [Required(ErrorMessage = "The Email field is required.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

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

        public EditProfileInputModel EditProfileInput { get; set; }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            try
            {
                Console.WriteLine("OnPostUpdateProfileAsync method is being called.");

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var email = Request.Form["EditProfileInput.Email"];
                var country = Request.Form["EditProfileInput.Country"];
                var state = Request.Form["EditProfileInput.State"];
                var city = Request.Form["EditProfileInput.City"];
                var zipCode = Request.Form["EditProfileInput.ZipCode"];
                var phoneNumber = Request.Form["EditProfileInput.PhoneNumber"];

                user.Email = email;
                user.Country = country;
                user.State = state;
                user.City = city;
                user.Zipcode = zipCode;
                user.PhoneNumber = phoneNumber;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "Information updated successfully.";
                    var updatedUser = await _userManager.GetUserAsync(User);
                    return RedirectToPage("/UserProfile");
                }
                else
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    Console.WriteLine("Update failed. Errors: " + string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return await OnGet();
        }



        public async Task<IActionResult> OnPostUploadImageAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            Console.WriteLine("User Id:======================= " + ApplicationUser.Id);


            if (FileInput != null && FileInput.Length > 0)
            {
                await AzureBlobUtil.UploadToBlob(FileInput, ApplicationUser.Id, "icons", ".jpg");
            }
            return RedirectToPage("/UserProfile");
        }

        public async Task<IActionResult> OnPostUploadCvAsync()
        {
            //upload CV
            if (FileInputCv != null && FileInputCv.Length > 0)
            {
                ApplicationUser = await _userManager.GetUserAsync(User);
                await AzureBlobUtil.UploadToBlob(FileInputCv, ApplicationUser.Id, "cvs", ".pdf");
            }
            //upload CV code ends
            return await OnGet();
        }

    }
}
