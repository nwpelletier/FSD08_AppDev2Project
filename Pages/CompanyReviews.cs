using System.Collections.Generic;
using FSD08_AppDev2Project.Models;
using FSD08_AppDev2Project.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.ObjectPool; // Add this using directive

namespace FSD08_AppDev2Project.Pages
{
    public class CompanyReviewsModel : PageModel
    {
        private readonly AppDev2DbContext _db; // Make sure AppDev2DbContext is accessible

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly List<CompanyModel> _companysModel;
        private readonly List<ApplicationUser> _ApplicationUsers ;
      [BindProperty]
        public List<Company> Companys { get; set; }
        
        [BindProperty]
        public int SelectedCompanyId { get; set; }

        [BindProperty]
        public List<ApplicationUser> ApplicationUsers { get; set; }
     
        public class CompanyModel{
            public Company Company { get; set; }
            public List<Review> Reviews { get; set; }
            public float aveRating {get; set;}
        }

        public List<Review> Reviews { get; set; }
        
         public List<CompanyModel> companysModel { get; set; } = new  List<CompanyModel> ();

        public CompanyReviewsModel(UserManager<ApplicationUser> userManager, AppDev2DbContext db)
        {
            _db = db;
            _userManager = userManager;
        }

        public async void OnGet(int? selectedCompanyId)
        {
            if (selectedCompanyId.HasValue){
                Reviews = _db.Reviews.Where(r => r.Company.Id == selectedCompanyId.Value).ToList();    
            }else {
                Reviews = new List<Review>();
            }
            
            Companys = _db.Companys.ToList();
        }

        public ActionResult OnPostGiveReview()
        {
             return RedirectToPage("LeaveComment");
        }

        
        public ActionResult OnPostSelectedCompanyReview(){
            return RedirectToPage("CompanyReviews", new {selectedCompanyId = SelectedCompanyId});;
        }
    }
}
