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
            var blob = await User.GetBlobAsync(principal, userContainer);
                        
            return new FileStreamResult(await blob.OpenReadAsync(), blob.Properties.ContentType);
        }
    }
}