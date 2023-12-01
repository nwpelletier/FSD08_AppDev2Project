using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FSD08_AppDev2Project.Pages
{
    public class LeaveCommentModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        
        public  async Task<IActionResult> OnGetAsync(string id)
        {
            if(id == null)
                return NotFound();
            return Page();
        }
    }
}
