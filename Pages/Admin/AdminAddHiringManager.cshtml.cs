using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;

namespace FSD08_AppDev2Project.Pages
{
    public class AddHiringManagerModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public int CompanyId { get; set; }

        public List<ApplicationUser> HiringManagers { get; set; }

        public AddHiringManagerModel(AppDev2DbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult OnGet(int companyId)
        {
            CompanyId = companyId;
            HiringManagers = _userManager.GetUsersInRoleAsync("HiringManager").Result.ToList();
            return Page();
        }


// TODO Needs fix
        // public async Task<IActionResult> OnPostAsync(List<string> selectedUserIds)
        // {
        //     var company = await _db.Companies.Include(c => c.HiringManagers).FirstOrDefaultAsync(c => c.Id == CompanyId);
        //     if (company == null)
        //     {
        //         return NotFound();
        //     }

        //     var selectedHiringManagers = await _userManager.Users.Where(u => selectedUserIds.Contains(u.Id)).ToListAsync();
        //     foreach (var hiringManager in selectedHiringManagers)
        //     {
        //         hiringManager.CompanyId = CompanyId;
        //     }

        //     await _db.SaveChangesAsync();

        //     return RedirectToPage("/Admin");
        // }
    }
}
