using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Azure.Identity;
using System.Net;

namespace FSD08_AppDev2Project.Data
{
    public class AzureBlobUtil
    {
        static string sasToken = "sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2023-12-30T07:38:42Z&st=2023-11-20T23:38:42Z&sip=1.0.0.0-223.255.255.255&spr=https&sig=nuduce9h%2FPmxk0N%2FphRowMjrR%2BU9fAFx%2FQ67nrjxAho%3D";

        // Storage Name:  indeedjob
        //Containers:  icons (To store user icons), cvs (to store CVs)

        public static string GetBlobUrl(string userId, string container, string extension)
        {
            Console.WriteLine("Inside Get blob===================================");
            try
            {
                string blobUrl = "";
                string blobName = userId + extension;
                string cacheBuster = DateTime.UtcNow.Ticks.ToString();
                Uri blobUri = new Uri($"https://indeedjob.blob.core.windows.net/{container}/{blobName}?{sasToken}&cb={cacheBuster}");
                //Uri blobUri = new Uri($"https://indeedjob.blob.core.windows.net/icons");
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToString() == "Development")
                {
                    Console.WriteLine("Inside Get blob Dev===================================");

                    var blobServiceClient = new BlobServiceClient(new UriBuilder(blobUri) { Query = sasToken }.Uri);
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(container);
                    BlobClient blobClient = containerClient.GetBlobClient(blobName);
                    blobUrl = blobClient.Uri.AbsoluteUri;
                    Console.WriteLine("BlobURI Dev ==================================" + blobUrl);
                }
                else
                {   //For App Service env
                    // Get a credential and create a client object for the blob container.

                    BlobContainerClient containerClient = new BlobContainerClient(blobUri, new DefaultAzureCredential());
                    BlobClient blobClient = containerClient.GetBlobClient(blobName);
                    blobUrl = blobClient.Uri.AbsoluteUri;
                    Console.WriteLine("BlobURI App Service ==================================" + blobUrl);
                }
                bool isValid = true; //IsValidBlobUri(blobUrl.ToString());

                if (isValid)
                {
                    return blobUrl.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception From Get Blob Method ============-=====" + e.Message);
                return string.Empty;
            }
        }

        // public static async Task<string> GetCVsBlobUrl(string userId)
        // {
        //     string blobName = userId + ".pdf";
        //     string cacheBuster = DateTime.UtcNow.Ticks.ToString();
        //     Uri blobUri = new Uri($"https://indeedjob.blob.core.windows.net/cvs/{blobName}?{sasToken}&cb={cacheBuster}");

        //     bool isValid = IsValidBlobUri(blobUri.ToString());

        //     if (isValid)
        //     {
        //         return blobUri.ToString();
        //     }
        //     else
        //     {
        //         return string.Empty;
        //     }

        // }

        public static async Task UploadToBlob(IFormFile InputFile, string userId, string container, string extension)
        {

            Uri containerUri = new Uri($"https://indeedjob.blob.core.windows.net/{container}");
            try
            {
                string fileName = Path.GetFileName(InputFile.FileName);
                string filePath = Path.Combine("wwwroot", "uploads", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await InputFile.CopyToAsync(stream);
                }
                using (var streamReader = new StreamReader(InputFile.OpenReadStream()))
                {
                    var blobContents = streamReader.ReadToEnd();
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToString() == "Development")
                    {   //For Development env
                        BlobServiceClient blobServiceClient = new BlobServiceClient(new UriBuilder(containerUri) { Query = sasToken }.Uri);
                        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(container);
                        BlobClient blobClient = containerClient.GetBlobClient(userId + extension);

                        using (FileStream fs = File.OpenRead(filePath))
                        {
                            blobClient.Upload(fs, true);
                        };
                    }
                    else
                    {   //For App Service env
                        // Get a credential and create a client object for the blob container.
                        
                        BlobServiceClient blobServiceClient = new BlobServiceClient(containerUri, new DefaultAzureCredential());
                        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(container);
                        BlobClient blobClient = containerClient.GetBlobClient(userId + extension);

                        using (FileStream fs = File.OpenRead(filePath))
                        {
                            blobClient.Upload(fs, true);
                        };


                        //Enable from here -----------
                        // string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
                        //                                 "indeedjob",
                        //                                 container);

                        // BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint),
                        //                                                     new DefaultAzureCredential());

                        //  // Create the container if it does not exist.
                        // await containerClient.CreateIfNotExistsAsync();


                        // byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);
                        // string newFileName = userId.ToString() + extension.ToString();
                        // using (MemoryStream stream = new MemoryStream(byteArray))
                        // {
                        //     await containerClient.UploadBlobAsync(newFileName, stream);
                        // }
                    }
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception From UPload Blob Method ============-=====" + e.Message);
            }

        }


        public static bool IsValidBlobUri(string blobUri)
        {
            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Head,
                    RequestUri = new Uri(blobUri)
                };

                HttpResponseMessage response;
                try
                {
                    response = client.Send(request);
                    return response.StatusCode == HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception From IsValidBlobUri Method:  =============" + ex.Message);
                    return false;
                }
            }
        }

        // public static async Task<bool> IsValidBlobUri(string blobUri)
        //     {
        //         using (HttpClient client = new HttpClient())
        //         {
        //             var request = new HttpRequestMessage
        //             {
        //                 Method = HttpMethod.Head,
        //                 RequestUri = new Uri(blobUri)
        //             };

        //             var response = await client.SendAsync(request);

        //             if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //             {
        //                 return true;
        //             }

        //             return false;
        //         }
        //     }

    }

}