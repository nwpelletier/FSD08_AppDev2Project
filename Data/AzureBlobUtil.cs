using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FSD08_AppDev2Project.Data
{
    public class AzureBlobUtil
    {
        static string sasToken = "sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2023-12-30T07:38:42Z&st=2023-11-20T23:38:42Z&sip=1.0.0.0-223.255.255.255&spr=https&sig=nuduce9h%2FPmxk0N%2FphRowMjrR%2BU9fAFx%2FQ67nrjxAho%3D";
        
        // Storage Name:  indeedjob
        //Containers:  icons (To store user icons), cvs (to store CVs)
        static string iconsContainer = "icons";
        static string cvsContainer = "cvs";

        public static void UploadIconToBlob(string localImagePath, string userId)
        {
            Uri containerUri = new Uri($"https://indeedjob.blob.core.windows.net/{iconsContainer}");
            BlobServiceClient blobServiceClient = new BlobServiceClient(new UriBuilder(containerUri) { Query = sasToken }.Uri);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(iconsContainer);
            BlobClient blobClient = containerClient.GetBlobClient(userId + ".jpg");

            using (FileStream fs = File.OpenRead(localImagePath))
            {
                blobClient.Upload(fs, true);
            };
        }
        public static string GetIconBlobUrl(string userId)
        {
            string blobName = userId + ".jpg";
            string cacheBuster = DateTime.UtcNow.Ticks.ToString();
            Uri blobUri = new Uri($"https://indeedjob.blob.core.windows.net/{iconsContainer}/{blobName}?{sasToken}&cb={cacheBuster}");
            return blobUri.ToString();
        }
        public static void UploadCVsToBlob(string localImagePath, string userId)
        {
            Uri containerUri = new Uri($"https://indeedjob.blob.core.windows.net/{cvsContainer}");
            BlobServiceClient blobServiceClient = new BlobServiceClient(new UriBuilder(containerUri) { Query = sasToken }.Uri);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(cvsContainer);
            BlobClient blobClient = containerClient.GetBlobClient(userId + ".pdf");

            using (FileStream fs = File.OpenRead(localImagePath))
            {
                blobClient.Upload(fs, true);
            };
        }
        public static string GetCVsBlobUrl(string userId)
        {
            string blobName = userId + ".pdf";
            string cacheBuster = DateTime.UtcNow.Ticks.ToString();
            Uri blobUri = new Uri($"https://indeedjob.blob.core.windows.net/{cvsContainer}/{blobName}?{sasToken}&cb={cacheBuster}");
            return blobUri.ToString();
        }
        
    }

}