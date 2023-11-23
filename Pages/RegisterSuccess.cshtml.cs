using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FSD08_AppDev2Project.Pages
{
    public class RegisterSuccessModel : PageModel
    {
        [BindProperty]
        public string email { get; set; }
        public void OnGet()
        {
        }
    }
}
