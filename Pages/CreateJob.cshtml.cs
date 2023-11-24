using System;
using System.ComponentModel.DataAnnotations;
using FSD08_AppDev2Project.Models;
using FSD08_AppDev2Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace FSD08_AppDev2Project.Pages
{
    // [Authorize]
    public class CreateJobModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly ILogger<CreateJobModel> _logger;

        public CreateJobModel(AppDev2DbContext db, ILogger<CreateJobModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "The Job Title field is required.")]
            [Display(Name = "Job Title")]
            public string JobTitle { get; set; }

            [Required(ErrorMessage = "The Job Category field is required.")]
            [Display(Name = "Job Category")]
            public string JobCategory { get; set; }

            [Required(ErrorMessage = "The Job Description field is required.")]
            [Display(Name = "Job Description")]
            public string JobDescription { get; set; }

            [Required(ErrorMessage = "The Start Date field is required.")]
            [DataType(DataType.Date)]
            [Display(Name = "Start Date")]
            public DateTime StartDate { get; set; }

            [Required(ErrorMessage = "The End Date field is required.")]
            [DataType(DataType.Date)]
            [Display(Name = "End Date")]
            public DateTime EndDate { get; set; }
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var newJob = new Job
                {
                    JobTitle = Input.JobTitle,
                    JobCategory = Input.JobCategory,
                    JobDescription = Input.JobDescription,
                    StartDate = Input.StartDate,
                    EndDate = Input.EndDate
                };

                _db.Jobs.Add(newJob);
                _db.SaveChanges();

                _logger.LogInformation($"Job '{Input.JobTitle}' created successfully.");

                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
