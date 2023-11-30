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

        public CompanyReviewsModel(UserManager<ApplicationUser> userManager, AppDev2DbContext db)
        {
            _db = db;
            _userManager = userManager;
        }
      [BindProperty]
        public List<Company> Companys { get; set; }
        // public List<Review> Reviews { get; set; }
        [BindProperty]
        public int SelectedCompanyId { get; set; }

        [BindProperty]
        public List<ApplicationUser> ApplicationUsers { get; set; }

     
        public class CompanyModel{
      [BindProperty]
            public Company Company { get; set; }
                  [BindProperty]
            public List<Review> Reviews { get; set; }
      [BindProperty]
            public float aveRating {get; set;}
        }
        public List<CompanyModel> companysModel = new List<CompanyModel>();
              [BindProperty]
        public List<CompanyModel> companysModelInput  { get; set; }

        public async void OnGet()
        {
            Companys = _db.Companys.ToList();
            ApplicationUsers = _db.ApplicationUsers.ToList();
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            foreach(var company in Companys){
                CompanyModel companyModel = new CompanyModel();
                companyModel.Company = company;
                companyModel.Reviews =  _db.Reviews.Where(re => re.Company == company).ToList();
                int SumR = 0;
                foreach(var r in companyModel.Reviews){
                    SumR += r.Stars;
                }
                if(companyModel.Reviews.Count() > 0)
                    companyModel.aveRating = SumR/companyModel.Reviews.Count();
                    else companyModel.aveRating = -1;
                companysModel.Add(companyModel);

            }

            if (currentUser != null)
            {

            }

        }

        public async Task<ActionResult> OnPostAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            
            Review review =new Review();
           
            _db.SaveChanges();


            return RedirectToPage("CompanyReviews");
        }
    }
}
