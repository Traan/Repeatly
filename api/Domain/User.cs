using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Repeatly.API.Domain
{
    public class User
    {
        public ClientPrincipal ClientPrincipal { get; set; }

        public Settings Settings { get; set; }

        public List<Task> Tasks { get; set; }

        public async static Task<CloudBlockBlob> GetBlobAsync(ClientPrincipal principal, CloudBlobContainer userContainer)
        {
            var blob = userContainer.GetBlockBlobReference(principal.UserId);
            var blobExists = await blob.ExistsAsync();
            if (!blobExists)
            {
                var user = new User
                {
                    ClientPrincipal = principal
                };

                var json = JsonConvert.SerializeObject(user);
                await blob.UploadTextAsync(json);

                blob.Properties.ContentType = "application/json";
                await blob.SetPropertiesAsync();
            }

            return blob;
        }
    }
}
