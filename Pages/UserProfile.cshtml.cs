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

        public ApplicationUser ApplicationUser { get; set; }
        public List<AppliedJob> AppliedJobs { get; set; }
        public List<Company> Companies { get; set; }

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

            return Page();
        }
    }
}
