using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using System.Threading.Tasks;

namespace FSD08_AppDev2Project.Pages
{
    public class AdminEditUserModel : PageModel
    {
        private readonly AppDev2DbContext _db;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public int Active { get; set; }

        public AdminEditUserModel(AppDev2DbContext db)
        {
            _db = db;
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
            Active = user.Active ? 1 : 0;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("INVALID");
                return Page();
            }

            var user = await _db.ApplicationUsers.FindAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var userName = Request.Form["UserName"];
            user.UserName = userName;

            user.Active = Active == 1;

            _db.SaveChanges();

            return RedirectToPage("Admin");
        }

    }
}
