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
        private readonly AppDev2DbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        public JobPostingsModel(UserManager<ApplicationUser> userManager, AppDev2DbContext db)
        {
            _db = db;
            _userManager = userManager;
        }

        // Pagination Setup (we can adjust # of postings per page here)
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public int TotalPages => (int)Math.Ceiling((double)_db.Jobs.Count() / PageSize);


        public List<Job> Jobs { get; set; }
        [BindProperty]
        public int SelectedJobId { get; set; }

        [BindProperty]
        public List<AppliedJob> AppliedJobs { get; set; }
        [BindProperty]
        public List<ApplicationUser> ApplicationUsers { get; set; }

        public async void OnGet()
        {
            var skipAmount = (CurrentPage - 1) * PageSize;

            Jobs = _db.Jobs.Skip(skipAmount).Take(PageSize).ToList();
            ApplicationUsers = _db.ApplicationUsers.ToList();
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                AppliedJobs = _db.AppliedJobs.Include(j => j.Applicant).Where(ja => ja.Applicant.Id == currentUser.Id).ToList();

                Console.WriteLine("TotalPages: " + TotalPages);
                Console.WriteLine("CurrentPage: " + CurrentPage);
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
