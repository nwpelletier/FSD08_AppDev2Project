using FSD08_AppDev2Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PexelsDotNetSDK;
using PexelsDotNetSDK.Api;
using PexelsDotNetSDK.Models;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FSD08_AppDev2Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // Property to store the selected landscape URL
        public string StockImageUrl { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                var pexelsClient = new PexelsClient("JkuN1YWaHHSEKvZ4gHFIw363G1F2hAKZcOKPnm1v7csZr9deoW5CNmIN");
                string[] searchWords = { "office", "workers", "workplace", "seminar", "working", "interview", "building", "carpentry", "teacher", "classroom", "professional", "desk" };
                Random random = new Random();
                string randomWord = searchWords[random.Next(searchWords.Length)];
                var result = await pexelsClient.SearchPhotosAsync(randomWord);
                if (result?.photos?.Count > 0)
                {
                    bool isValidPhotoId = false;

                    while (!isValidPhotoId)
                    {
                        int randomIndex = new Random().Next(0, Math.Min(10, result.photos.Count));

                        var randomPhoto = result.photos[randomIndex];

                        if (randomPhoto.id is int photoIdInt)
                        {
                            string photoId = photoIdInt.ToString();

                            if (photoId.Length >= 6)
                            {
                                StockImageUrl = $"https://images.pexels.com/photos/{photoId}/pexels-photo-{photoId}.jpeg";
                                Console.WriteLine(StockImageUrl);
                                isValidPhotoId = true;
                            }
                            else
                            {
                                Console.WriteLine($"Skipping photo with id {photoId} because its length is less than 6.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Skipping photo with id {randomPhoto.id} because it's not a valid integer.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No photos found in the result.");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
