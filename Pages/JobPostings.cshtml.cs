using System.Collections.Generic;
using FSD08_AppDev2Project.Models;
using FSD08_AppDev2Project.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization; // Add this using directive

namespace FSD08_AppDev2Project.Pages
{
    public class JobPostingsModel : PageModel
    {
        private readonly AppDev2DbContext _db; // Make sure AppDev2DbContext is accessible

        private readonly UserManager<ApplicationUser> _userManager;

        public JobPostingsModel(UserManager<ApplicationUser> userManager, AppDev2DbContext db)
        {
            _db = db;
            _userManager = userManager;
        }

        public List<Job> Jobs { get; set; }
        [BindProperty]
        public int SelectedJobId { get; set; }

        [BindProperty]
        public List<AppliedJob> AppliedJobs { get; set; }
        [BindProperty]
        public List<ApplicationUser> ApplicationUsers { get; set; }

        public async void OnGet()
        {
            Jobs = _db.Jobs.ToList();
            ApplicationUsers = _db.ApplicationUsers.ToList();
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                AppliedJobs = _db.AppliedJobs.Include(j => j.Applicant).Where(ja => ja.Applicant.Id == currentUser.Id).ToList();

                Console.WriteLine("JobsJSOnuser*----" + JsonSerializer.Serialize(AppliedJobs));
                // Console.WriteLine("CurrentUser*----" + JsonSerializer.Serialize(currentUser));
            }
        }

        public async Task<ActionResult> OnPostAsync()
        {
            Job selectedJob = _db.Jobs.Find(SelectedJobId);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            AppliedJob appliedJob = new AppliedJob()
            {
                Applicant = user,
                Job = selectedJob,
                AppliedDate = DateTime.Now
            };

            _db.AppliedJobs.Add(appliedJob);
            _db.SaveChanges();


            TempData["JobMessage"] = "Job added:  Your details are send to employer!";
            return RedirectToPage("JobPostings");
        }
    }
}
