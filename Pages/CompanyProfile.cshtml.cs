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
        public CompanyProfileModel(SignInManager<ApplicationUser> signInManager, AppDev2DbContext db, ILogger<CompanyProfileModel> logger, IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Company Company { get; set; }
        public List<Job> Jobs { get; set; }
        public List<string> CompanyLogoUrls { get; set; }

        public async Task OnGetAsync(int companyId){
            ApplicationUser = await _userManager.GetUserAsync(User);

            Company = _db.Companys.FirstOrDefault(c => c.HiringManagers[0].UserName == ApplicationUser.UserName);
            if(Company != null){
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
        }
        
        public ActionResult OnPostEditCompany()
        {
            return RedirectToPage("EditCompany");
        }
    }

}
