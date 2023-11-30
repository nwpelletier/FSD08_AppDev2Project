using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FSD08_AppDev2Project.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminEditUserRoleModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public List<string> Roles { get; set; } = new List<string>();

        public List<IdentityRole> AvailableRoles { get; set; } = new List<IdentityRole>();

        public AdminEditUserRoleModel(AppDev2DbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.ApplicationUsers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            UserName = user.UserName;
            Roles = (await _userManager.GetRolesAsync(user)).ToList();
            AvailableRoles = await _roleManager.Roles.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AvailableRoles = await _roleManager.Roles.ToListAsync();
                return Page();
            }

            var user = await _db.ApplicationUsers.FindAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            // This is setup so that it will delete ALL roles, then re-append the new role
            // Avoids multiple roles for users, also so that I don't have to add in "Delete Role"
            var userName = Request.Form["UserName"];
            user.UserName = userName;

            var existingRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            var selectedRoles = Request.Form["Roles"];
            await _userManager.AddToRolesAsync(user, selectedRoles);

            return RedirectToPage("Admin");
        }

    }
}
