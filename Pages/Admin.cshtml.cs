using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSD08_AppDev2Project.Pages
{
    public class AdminModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public AdminModel(AppDev2DbContext db)
        {
            _db = db;
        }
        public List<Company> Companys { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Job> Jobs { get; set; }

        public void OnGet()
        {
            Companys = _db.Companys.Include(c => c.HiringManagers).ToList();
            ApplicationUsers = _db.ApplicationUsers.ToList();
            Reviews = _db.Reviews.ToList();
            Jobs = _db.Jobs.ToList();
        }
    }
}
