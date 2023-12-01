using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using FSD08_AppDev2Project.Data;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FSD08_AppDev2Project.Pages
{
     [Authorize]
    public class LeaveCommentModel : PageModel
    {
        private readonly AppDev2DbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaveCommentModel(AppDev2DbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager; 
        }

        public Review Reviews { get; set; }
         public Company Company { get; set; }

        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        
        public IActionResult OnGet(string id)
        {            
            if(id == null)
                return NotFound();
            Company = _db.Companys.FirstOrDefault(c => c.Id == int.Parse(id));
            return Page();
        }
        // public async Task<ActionResult> OnPost()
        // {
        //     ApplicationUser currentUser = await _userManager.GetUserAsync(User);
        //     if(currentUser != null){
        //         Review newReview = new Review();
        //         newReview.Company = Company;
        //         newReview.Reviews = Reviews.Reviews;
        //         newReview.Stars = 4;
        //         newReview.User = currentUser;

        //         _db.Reviews.Add(newReview);
        //         _db.SaveChanges();
        //     }
        //     return Page();
        // }
    }
}
