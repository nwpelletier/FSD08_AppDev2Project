using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace FSD08_AppDev2Project.Pages
{
    [Authorize]
    public class LeaveCommentModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public List<Company> Companys { get; set; }
        [BindProperty]
        public int SelectedCompanyId { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Please provide reviews.")]
            public string Reviews { get; set; }

            [Required(ErrorMessage = "Please provide stars.")]
            public int Stars { get; set; }

        }
        public LeaveCommentModel(AppDev2DbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public Review Reviews { get; set; }
        public Company Company { get; set; }

        // [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        public void OnGet()
        {
            this.Companys = _db.Companys.ToList();
        }

        public async Task<ActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var newReview = new Review
                {
                    User = await _userManager.GetUserAsync(User),
                    Company = _db.Companys.Find(SelectedCompanyId),
                    Reviews = Input.Reviews,
                    Stars = Input.Stars
                };

                _db.Reviews.Add(newReview);
                _db.SaveChanges();

                return RedirectToPage("CompanyReviews");
            }

            return Page();

        }
    }
}
