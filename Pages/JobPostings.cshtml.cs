using System.Collections.Generic;
using FSD08_AppDev2Project.Models;
using FSD08_AppDev2Project.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.Net.WebSockets; // Add this using directive

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
        public int TotalPages { get; set; }
        public String roleTemp { get; set; }
        public List<Job> Jobs { get; set; }
        [BindProperty]
        public int SelectedJobId { get; set; }
        public Role UserRole { get; set; }
        public ApplicationUser currentUser { get; set; }
        [BindProperty]
        public List<AppliedJob> AppliedJobs { get; set; }
        [BindProperty]
        public List<ApplicationUser> ApplicationUsers { get; set; }
        [BindProperty]
        public List<Job> AllJobs { get; set; }
        [BindProperty]
        public bool IsAuthenticated { get; set; }
        [BindProperty]
        public List<Company> Companys { get; set; }

        [BindProperty]
        public int SelectedCompanyId { get; set; }

        public async void OnGet(int? currentPage, int? selectedCompanyId)
        {
            try
            {
                var skipAmount = (CurrentPage - 1) * PageSize;
                Companys = _db.Companys.ToList();
                //Stay on same page after apply
                if (currentPage.HasValue)
                {
                    CurrentPage = currentPage.Value;
                }
                if (selectedCompanyId.HasValue && selectedCompanyId.Value != 0)
                { //Show JObs for selected Company in dropdown
                    SelectedCompanyId = selectedCompanyId.Value;
                    TotalPages = (int)Math.Ceiling((double)_db.Jobs.Where(j => j.JobCompanyId == SelectedCompanyId).Count() / PageSize);

                    Jobs = _db.Jobs.Where(j => j.JobCompanyId == SelectedCompanyId).Skip(skipAmount).Take(PageSize).ToList();
                }
                else
                { //Show all Jobs
                    TotalPages = (int)Math.Ceiling((double)_db.Jobs.Count() / PageSize);
                    Jobs = _db.Jobs.Skip(skipAmount).Take(PageSize).ToList();
                }

                AllJobs = _db.Jobs.ToList();
                ApplicationUsers = _db.ApplicationUsers.ToList();
                currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    AppliedJobs = _db.AppliedJobs.Include(j => j.Applicant).Where(ja => ja.Applicant.Id == currentUser.Id).ToList();

                    Console.WriteLine("TotalPages: " + TotalPages);
                    Console.WriteLine("CurrentPage: " + CurrentPage);
                    IsAuthenticated = true;
                }
                else
                {
                    IsAuthenticated = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("OnGet Exception JOb Posting ==========================================================" + ex.Message);
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

            return RedirectToPage("JobPostings", new { currentPage = CurrentPage, selectedCompanyId = SelectedCompanyId });
        }

        public async Task<ActionResult> OnPostFindByCompanyAsync()
        {
            //If find by Company clicked, go to page 1
            return RedirectToPage("JobPostings", new { currentPage = 1, selectedCompanyId = SelectedCompanyId });
        }

    }
}
