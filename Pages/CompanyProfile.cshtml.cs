using System.Collections.Generic;
using System.Linq;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging; // Add this using statement

namespace FSD08_AppDev2Project.Pages
{
    public class CompanyProfileModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly ILogger<CompanyProfileModel> _logger; // Add this field

        public CompanyProfileModel(AppDev2DbContext db, ILogger<CompanyProfileModel> logger)
        {
            _db = db;
            _logger = logger; // Initialize the logger
        }

        public Company Company { get; set; }
        public List<Job> Jobs { get; set; }

        public void OnGet(int companyId)
        {
            // Retrieve company information
            Company = _db.Companys.FirstOrDefault(c => c.Id == companyId);

            // Log the company name
            _logger.LogInformation($"Company Name: {Company?.Name}");

            // Retrieve job postings for the company
            Jobs = _db.Jobs.Where(j => j.JobCompany.Id == companyId).ToList();
        }
        
    }
}
