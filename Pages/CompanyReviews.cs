using System.Collections.Generic;
using FSD08_AppDev2Project.Models;
using FSD08_AppDev2Project.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.ObjectPool; // Add this using directive

namespace FSD08_AppDev2Project.Pages
{
    public class CompanyReviewsModel : PageModel
    {
        private readonly AppDev2DbContext _db; // Make sure AppDev2DbContext is accessible

        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyReviewsModel(UserManager<ApplicationUser> userManager, AppDev2DbContext db)
        {
            _db = db;
            _userManager = userManager;
        }

        public List<Company> Companys { get; set; }
        // public List<Review> Reviews { get; set; }
        [BindProperty]
        public int SelectedCompanyId { get; set; }

        [BindProperty]
        public List<ApplicationUser> ApplicationUsers { get; set; }

        public class CompanyModel{
            public Company Company { get; set; }
            public List<Review> Reviews { get; set; }

        }
        public List<CompanyModel> companysModel = new List<CompanyModel>();

        public async void OnGet()
        {
            Companys = _db.Companys.ToList();
            ApplicationUsers = _db.ApplicationUsers.ToList();
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            foreach(var company in Companys){
                CompanyModel companyModel = new CompanyModel();
                companyModel.Company = company;
                companyModel.Reviews =  _db.Reviews.Where(re => re.Company == company).ToList();
                companysModel.Add(companyModel);
            }

            if (currentUser != null)
            {

            }

        }

        // public async Task<ActionResult> OnPostAsync()
        // {
        //     Job selectedJob = _db.Jobs.Find(SelectedJobId);
        //     ApplicationUser user = await _userManager.GetUserAsync(User);
        //     AppliedJob appliedJob = new AppliedJob()
        //     {
        //         Applicant = user,
        //         Job = selectedJob,
        //         AppliedDate = DateTime.Now
        //     };

        //     _db.AppliedJobs.Add(appliedJob);
        //     _db.SaveChanges();


        //     TempData["JobMessage"] = "Job added:  Your details are send to employer!";
        //     return RedirectToPage("JobPostings");
        // }
    }
}
