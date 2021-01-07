using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Repeatly.API.Domain;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Repeatly.API
{
    [StorageAccount("STORAGE_ACCOUNT")]
    public class UserFunction
    {
        private const string FUNCTION_NAME = "user";

        [FunctionName(FUNCTION_NAME)]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", "POST")] HttpRequest request,
            [Blob(FUNCTION_NAME, FileAccess.ReadWrite)] CloudBlobContainer userContainer,
            ILogger logger)
        {
            var blob = request.Method switch
            {
                "GET" => await User.GetBlobAsync(request, userContainer),
                "POST" => await User.SaveAsync(request, userContainer),
                _ => null
            };

            return new FileStreamResult(await blob.OpenReadAsync(), blob.Properties.ContentType);
        }
    }
}