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

namespace FSD08_AppDev2Project.Pages
{
    [Authorize]
    [Authorize(Roles = "HiringManager")]
    public class CompanyProfileModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly ILogger<CompanyProfileModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CompanyProfileModel(AppDev2DbContext db, ILogger<CompanyProfileModel> logger, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public Company Company { get; set; }
        public List<Job> Jobs { get; set; }
        public List<string> CompanyLogoUrls { get; set; }

        public async Task OnGetAsync(int companyId)
        {
            Company = _db.Companys.FirstOrDefault(c => c.Id == companyId);

            _logger.LogInformation($"Company Name: {Company?.Name}");

            Jobs = _db.Jobs.Where(j => j.JobCompanyId == companyId).ToList();
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
}
