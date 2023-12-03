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
    public class ApplicantProfileModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicantProfileModel(AppDev2DbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public ApplicationUser ApplicationUser { get; set; }
        public List<AppliedJob> AppliedJobs { get; set; }
        public List<Company> Companies { get; set; }
        [BindProperty]
        public string ProfileImage { get; set; }
        [BindProperty]
        public string UserCV { get; set; }

        public string GetCompanyName(int companyId)
        {
            var company = Companies.FirstOrDefault(c => c.Id == companyId);
            return company != null ? company.Name : "Unknown Company";
        }

        public async Task<IActionResult> OnGet(string userId)
        {
            ApplicationUser = await _userManager.FindByIdAsync(userId);
            
            string iconImageURL = AzureBlobUtil.GetBlobUrl(ApplicationUser.Id, "icons", ".jpg");
            string cvURL = AzureBlobUtil.GetBlobUrl(ApplicationUser.Id, "cvs", ".pdf");

            AppliedJobs = _db.AppliedJobs.Include(j => j.Job).Where(ja => ja.Applicant.Id == ApplicationUser.Id).ToList();
            Companies = _db.Companys.ToList();

            ProfileImage = iconImageURL.ToString();
            UserCV = cvURL.ToString();

            Console.WriteLine("USserCV==================" + UserCV);
            Console.WriteLine("ProfileImage==================" + ProfileImage);
            Console.WriteLine("Environment=============================================" + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            return Page();
        }
    }
}
