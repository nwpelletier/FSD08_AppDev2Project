using System.Linq;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FSD08_AppDev2Project.Pages
{
    public class EditJobModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly ILogger<EditJobModel> _logger;

        public EditJobModel(AppDev2DbContext db, ILogger<EditJobModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        [BindProperty]
        public Job Job { get; set; }

        public IActionResult OnGet(int jobId, int companyId)
        {
            var company = _db.Companys.FirstOrDefault(c => c.Id == companyId);

            if (company == null)
            {
                _logger.LogWarning($"Company with Id {companyId} not found.");
                return NotFound();
            }

            Job = _db.Jobs
                .Where(j => j.Id == jobId && j.JobCompany.Id == companyId)
                .FirstOrDefault();

            if (Job == null)
            {
                _logger.LogWarning($"Job with JobId {jobId} not found or does not belong to CompanyId {companyId}.");
                return NotFound();
            }

            Job.JobCompany = company;

            return Page();
        }



        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is not valid.");
                return Page();
            }

            var job = _db.Jobs.FirstOrDefault(j => j.Id == Job.Id);

            if (job != null)
            {
                job.JobDescription = Job.JobDescription;
                _db.SaveChanges();

                _logger.LogInformation($"Job description updated successfully for JobId {Job.Id}.");
            }
            else
            {
                _logger.LogWarning($"Job with JobId {Job.Id} not found.");
                return NotFound();
            }

            return RedirectToPage("/CompanyProfile", new { companyId = job.JobCompany.Id });
        }
    }
}
