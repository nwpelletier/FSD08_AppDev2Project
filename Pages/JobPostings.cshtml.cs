using System.Collections.Generic;
using FSD08_AppDev2Project.Models;
using FSD08_AppDev2Project.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // Add this using directive

namespace FSD08_AppDev2Project.Pages
{
    public class JobPostingsModel : PageModel
    {
        private readonly AppDev2DbContext _db; // Make sure AppDev2DbContext is accessible

        public JobPostingsModel(AppDev2DbContext db)
        {
            _db = db;
        }

        public List<Job> Jobs { get; set; }

        public void OnGet()
        {
            Jobs = _db.Jobs.ToList();
        }
    }
}
