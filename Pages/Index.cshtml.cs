using FSD08_AppDev2Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace FSD08_AppDev2Project.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public IFormFile FileInput { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        //Console.WriteLine(AzureBlobUtil.GetIconBlobUrl("0c840a6c-f289-4a0a-8b6d-0a2a01912ecc"));
        //Console.WriteLine(AzureBlobUtil.GetCVsBlobUrl("0c840a6c-f289-4a0a-8b6d-0a2a01912ecc"));
    }

    public async void OnPostAsync()
        {
            if (FileInput != null && FileInput.Length > 0)
            {
                string fileName = Path.GetFileName(FileInput.FileName);

                string filePath = Path.Combine("wwwroot", "uploads", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await FileInput.CopyToAsync(stream);
                }

                //Temporary hardcoded userid
                //AzureBlobUtil.UploadIconToBlob(filePath, "0c840a6c-f289-4a0a-8b6d-0a2a01912ecc");
                AzureBlobUtil.UploadCVsToBlob(filePath, "0c840a6c-f289-4a0a-8b6d-0a2a01912ecc");

                System.IO.File.Delete(filePath);

                //return RedirectToPage("/Index"); // Redirect to the Index page
            }
            //return Page();
        }
}
