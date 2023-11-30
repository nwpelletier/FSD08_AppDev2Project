using System.Collections.Generic;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FSD08_AppDev2Project.Pages
{
    public class AdminModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminModel(AppDev2DbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager; 
        }

        public List<ApplicationUser> ApplicationUsers { get; set; }
        public Dictionary<string, List<string>> UserRoles { get; set; }
        public List<Company> Companys { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Job> Jobs { get; set; }

        public void OnGet()
        {
            Companys = _db.Companys.Include(c => c.HiringManagers).ToList();
            ApplicationUsers = _db.ApplicationUsers.ToList();
            Reviews = _db.Reviews.ToList();
            Jobs = _db.Jobs.ToList();

            UserRoles = new Dictionary<string, List<string>>();
            foreach (var user in ApplicationUsers)
            {
                var roles = _userManager.GetRolesAsync(user).Result;
                UserRoles.Add(user.Id, roles.ToList());
            }
        }
    }
}
