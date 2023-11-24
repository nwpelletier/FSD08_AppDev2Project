using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace FSD08_AppDev2Project.Pages
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileModel(AppDev2DbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGet()
        {
            ApplicationUser = await _userManager.GetUserAsync(User);

// TODO: uncomment this once login persists
            // if (ApplicationUser == null)
            // {
            //     return RedirectToPage("Index");
            // }
            return Page();
        }
    }
}
