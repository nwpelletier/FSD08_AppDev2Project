using System.Collections.Generic;
using System.Linq;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace FSD08_AppDev2Project.Pages
{
    //[Authorize]
    [Authorize(Roles = "HiringManager")]
    public class CompanyProfileModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly ILogger<CompanyProfileModel> _logger;

        public CompanyProfileModel(AppDev2DbContext db, ILogger<CompanyProfileModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Company Company { get; set; }
        public List<Job> Jobs { get; set; }

        public void OnGet(int companyId)
        {
            Company = _db.Companys.FirstOrDefault(c => c.Id == companyId);

            _logger.LogInformation($"Company Name: {Company?.Name}");

            Jobs = _db.Jobs.Where(j => j.JobCompanyId == companyId).ToList();
        }
    }
}
