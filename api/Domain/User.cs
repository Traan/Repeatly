using Microsoft.AspNetCore.Http;
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

        public async static Task<CloudBlockBlob> GetBlobAsync(HttpRequest request, CloudBlobContainer userContainer)
        {
            var principal = await StaticWebAppsAuth.GetClientPrincipalAsync(request);
            var blob = GetBlob(principal, userContainer);
            var blobExists = await blob.ExistsAsync();
            if (!blobExists)
            {
                var user = new User
                {
                    ClientPrincipal = principal
                };

                await SaveAsync(blob, user);
            }

            return blob;
        }

        private static CloudBlockBlob GetBlob(ClientPrincipal principal, CloudBlobContainer userContainer)
        {
            return userContainer.GetBlockBlobReference(principal.UserId);
        }

        public async static Task<CloudBlockBlob> SaveAsync(HttpRequest request, CloudBlobContainer userContainer)
        {
            var principal = await StaticWebAppsAuth.GetClientPrincipalAsync(request);
            var blob = GetBlob(principal, userContainer);

            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<User>(requestBody);
            user.ClientPrincipal = principal;

            await SaveAsync(blob, user);

            return blob;
        }

        private static async System.Threading.Tasks.Task SaveAsync(CloudBlockBlob blob, User user)
        {
            var json = JsonConvert.SerializeObject(user);
            await blob.UploadTextAsync(json);

            blob.Properties.ContentType = "application/json";
            await blob.SetPropertiesAsync();
        }
    }
}
