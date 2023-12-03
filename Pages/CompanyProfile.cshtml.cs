using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FSD08_AppDev2Project.Pages
{
    [Authorize]
    [Authorize(Roles = "HiringManager")]
    public class CompanyProfileModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CompanyProfileModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationUser ApplicationUser { get; set; }
        public Company Company { get; set; }
        public List<Job> Jobs { get; set; }
        public List<string> CompanyLogoUrls { get; set; }
        public Dictionary<int, List<AppliedJob>> AppliedJobsForJobs { get; set; }

        public CompanyProfileModel(SignInManager<ApplicationUser> signInManager, AppDev2DbContext db, ILogger<CompanyProfileModel> logger, IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync(int companyId)
        {
            ApplicationUser = await _userManager.GetUserAsync(User);

            Company = _db.Companys.FirstOrDefault(c => c.HiringManagers[0].UserName == ApplicationUser.UserName);
            if (Company != null)
            {
                _logger.LogInformation($"Company Name: {Company?.Name}");

                Jobs = _db.Jobs.Where(j => j.JobCompanyId == Company.Id).ToList();
                CompanyLogoUrls = new List<string>();

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    string apiUrl = $"https://api.api-ninjas.com/v1/logo?name={Company.Name}";
                    httpClient.DefaultRequestHeaders.Add("X-Api-Key", "Qbk39pjWMzGyojJAuOX8QA==DyZF5iYnPYo2tlB6");

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        JArray logoDataArray = JsonConvert.DeserializeObject<JArray>(result);

                        foreach (var logoData in logoDataArray)
                        {
                            string imageUrl = logoData["image"]?.ToString();
                            CompanyLogoUrls.Add(imageUrl);
                        }
                    }
                    else
                    {
                        _logger.LogError($"Error retrieving company logo: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }

            AppliedJobsForJobs = new Dictionary<int, List<AppliedJob>>();
            foreach (var job in Jobs)
            {
                AppliedJobsForJobs[job.Id] = GetAppliedJobsForJob(job.Id);
            }
        }

        public ActionResult OnPostEditCompany()
        {
            return RedirectToPage("EditCompany");
        }

        public List<AppliedJob> GetAppliedJobsForJob(int jobId)
        {
            return _db.AppliedJobs
                .Include(aj => aj.Applicant)
                .Where(aj => aj.Job.Id == jobId)
                .ToList();
        }

        public class EditCompanyInputModel
        {
            [Required(ErrorMessage = "The Country field is required.")]
            [Display(Name = "Country")]
            public string Country { get; set; }

            [Required(ErrorMessage = "The State field is required.")]
            [Display(Name = "State")]
            public string State { get; set; }

            [Required(ErrorMessage = "The City field is required.")]
            [Display(Name = "City")]
            public string City { get; set; }
        }
        public EditCompanyInputModel EditCompanyInput { get; set; }
        public async Task<IActionResult> OnPostUpdateCompanyAsync(){
            ApplicationUser = await _userManager.GetUserAsync(User);
            Company = _db.Companys.FirstOrDefault(c => c.HiringManagers[0].UserName == ApplicationUser.UserName);

            Company.Country = Request.Form["EditCompanyInput.Country"];
            Company.State = Request.Form["EditCompanyInput.State"];
            Company.City = Request.Form["EditCompanyInput.City"];

            if (ModelState.IsValid)
            {
            var UpdateCompany = _db.Companys.FirstOrDefault(c => c.Id == Company.Id);

            if (UpdateCompany != null && Company.Country!= null && Company.State != null && Company.City != null)
            {
                UpdateCompany.Country = Company.Country;
                UpdateCompany.State = Company.State;
                UpdateCompany.City = Company.City;
                _db.SaveChanges();

                _logger.LogInformation($"Company Info description updated successfully for Company {Company.Name}.");
            }
            else
            {
                _logger.LogWarning($"Company with comapny name {Company.Name} not found.");
                return NotFound();
            }
                return RedirectToPage("/CompanyProfile");
            }
            return Page();
        }
    }
}
