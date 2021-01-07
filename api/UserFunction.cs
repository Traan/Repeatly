using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Repeatly.API
{
    [StorageAccount("STORAGE_ACCOUNT")]
    public partial class UserFunction
    {
        private const string FUNCTION_NAME = "user";

        [FunctionName(FUNCTION_NAME)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request,
            [Blob(FUNCTION_NAME, FileAccess.ReadWrite)] CloudBlobContainer userContainer,
            ILogger logger)
        {
            var principal = await StaticWebAppsAuth.GetClientPrincipalAsync(request);            
            var blob = userContainer.GetBlockBlobReference(principal.UserId);
            var blobExists = await blob.ExistsAsync();
            if (!blobExists)            
            {
                var json = JsonConvert.SerializeObject(principal);
                await blob.UploadTextAsync(json);
                
                blob.Properties.ContentType = "application/json";
                await blob.SetPropertiesAsync();
            }
                        
            return new FileStreamResult(await blob.OpenReadAsync(), blob.Properties.ContentType);
        }
    }
}