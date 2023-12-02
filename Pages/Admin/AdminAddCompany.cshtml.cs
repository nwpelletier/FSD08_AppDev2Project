using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;

namespace FSD08_AppDev2Project.Pages
{
    public class AdminAddCompanyModel : PageModel
    {
        private readonly AppDev2DbContext _db;

        [BindProperty]
        public Company Company { get; set; }

        public AdminAddCompanyModel(AppDev2DbContext db)
        {
            _db = db;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (_db.Companys.Any(c => c.Name == Company.Name))
            {
                 TempData["ErrorMessage"] = "Company already exists in database";
                return RedirectToPage("/Admin/Admin");
            }

            if (ModelState.IsValid)
            {
                _db.Companys.Add(Company);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Company added successfully";
                return RedirectToPage("/Admin/Admin");
            }

            return Page();
        }
    }

}
