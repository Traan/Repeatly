using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Repeatly.API.Domain;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Repeatly.API
{
    public static class StaticWebAppsAuth
    {
        public static async Task<ClientPrincipal> GetClientPrincipalAsync(HttpRequest request)
        {
            var isLocal = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
            var principal = isLocal
                ? new ClientPrincipal { UserId = "local", IdentityProvider = "localhost", UserDetails = "local", UserRoles = new[] { "authenticated" } }
                : await ParseAsync(request);

            return principal;
        }

        private static async Task<ClientPrincipal> ParseAsync(HttpRequest request)
        {
            var principal = new ClientPrincipal();

            if (request.Headers.TryGetValue("x-ms-client-principal", out var header))
            {
                var data = header[0];
                var decoded = Convert.FromBase64String(data);
                var json = Encoding.ASCII.GetString(decoded);

                principal = JsonConvert.DeserializeObject<ClientPrincipal>(json);
            }

            principal.UserRoles = principal.UserRoles?.Except(new string[] { "anonymous" }, StringComparer.CurrentCultureIgnoreCase);

            return await Task.FromResult(principal);
        }
    }
}