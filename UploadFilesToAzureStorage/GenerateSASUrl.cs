using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UploadFilesToAzureStorage
{
    public static class GenerateSASUrl
    {
        [FunctionName("GenerateSASUrl")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("StorageAccountKey"));

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("ContainerName"));

            if(!(await blobContainerClient.ExistsAsync()))
            {
                return new OkObjectResult(null);
            }

            var blobFile = blobContainerClient.GetBlobClient($"{Path.GetRandomFileName()}.bak");

            if(blobFile.CanGenerateSasUri)
            {
                var sasBuilder = new BlobSasBuilder(BlobContainerSasPermissions.Create | BlobContainerSasPermissions.Write, DateTimeOffset.UtcNow.AddHours(1))
                {
                    BlobContainerName = blobFile.BlobContainerName,
                    BlobName = blobFile.Name,
                    Resource = "b"
                };

                var sasUri = blobFile.GenerateSasUri(sasBuilder);

                return new OkObjectResult(sasUri.ToString());
            }
            else
            {
                return new OkObjectResult(null);
            }
        }
    }
}