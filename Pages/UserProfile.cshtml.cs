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

            AppliedJobs = _db.AppliedJobs.Include(j => j.Job).Where(ja => ja.Applicant.Id == ApplicationUser.Id).ToList();

            Companies = _db.Companys.ToList();

            ProfileImage = await AzureBlobUtil.GetIconBlobUrl(ApplicationUser.Id);
            UserCV = await AzureBlobUtil.GetCVsBlobUrl(ApplicationUser.Id);
            Console.WriteLine("USserCV==================" + UserCV);
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
            ApplicationUser = await _userManager.GetUserAsync(User);
            EditProfileInput = new EditProfileInputModel();

            EditProfileInput.Email = Request.Form["EditProfileInput.Email"];
            EditProfileInput.Country = Request.Form["EditProfileInput.Country"];
            EditProfileInput.State = Request.Form["EditProfileInput.State"];
            EditProfileInput.City = Request.Form["EditProfileInput.City"];
            EditProfileInput.ZipCode = Request.Form["EditProfileInput.ZipCode"];
            EditProfileInput.PhoneNumber = Request.Form["EditProfileInput.PhoneNumber"];

            if (ModelState.IsValid)
            {
                ApplicationUser.Email = EditProfileInput.Email;
                ApplicationUser.Country = EditProfileInput.Country;
                ApplicationUser.State = EditProfileInput.State;
                ApplicationUser.City = EditProfileInput.City;
                ApplicationUser.Zipcode = EditProfileInput.ZipCode;
                ApplicationUser.PhoneNumber = EditProfileInput.PhoneNumber;

                var result = await _userManager.UpdateAsync(ApplicationUser);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Information updated successfully.";

                    return RedirectToPage("/UserProfile");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return Page();
        }

        // Separate Async Post method from edit profile
        // Need to troubleshoot this - it's grabbing proper filename, correct ApplicationUser.Id
        public async Task<IActionResult> OnPostUploadImageAsync()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            Console.WriteLine("=============================");
            Console.WriteLine("User Id: " + ApplicationUser.Id);
            Console.WriteLine("User Name: " + ApplicationUser.UserName);
            Console.WriteLine("=============================");

            if (ApplicationUser != null)
            {
                Console.WriteLine($"File Input: {FileInput.FileName}");
                if (FileInput != null && FileInput.Length > 0)
                {
                    Console.WriteLine("=============FileInputDetected=============");
                    string fileName = Path.GetFileName(FileInput.FileName);
                    string filePath = Path.Combine("wwwroot", "uploads", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FileInput.CopyToAsync(stream);
                    }
                    AzureBlobUtil.UploadCVsToBlob(filePath, ApplicationUser.Id);
                    Console.WriteLine(filePath);
                    System.IO.File.Delete(filePath);
                }
                return RedirectToPage("/UserProfile");
            }
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostUploadCvAsync()
        {
            //upload CV
            if (FileInputCv != null && FileInputCv.Length > 0)
            {
                ApplicationUser = await _userManager.GetUserAsync(User);

                string fileName = Path.GetFileName(FileInputCv.FileName);
                string filePath = Path.Combine("wwwroot", "uploads", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await FileInputCv.CopyToAsync(stream);
                }
                AzureBlobUtil.UploadCVsToBlob(filePath, ApplicationUser.Id);
                System.IO.File.Delete(filePath);
            }
            //upload CV code ends
            return await OnGet();
        }

    }
}
