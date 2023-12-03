using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FSD08_AppDev2Project.Pages.Admin
{
    public class AdminAddUserCompanyModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminAddUserCompanyModel(AppDev2DbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty]
        public string UserId { get; set; }

        [BindProperty]
        public int CompanyId { get; set; }

        public List<SelectListItem> Companies { get; set; }

        public async Task OnGetAsync(string id)
        {
            UserId = id;

            Companies = await _db.Companys
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserId);

            if (user != null)
            {
                user.CompanyId = CompanyId;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToPage("/Admin/Admin");
        }
    }
}
