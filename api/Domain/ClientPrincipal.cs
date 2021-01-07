using System.Collections.Generic;

namespace Repeatly.API.Domain
{
    public class ClientPrincipal
    {
        public string IdentityProvider { get; set; }

        public string UserId { get; set; }

        public string UserDetails { get; set; }

        public IEnumerable<string> UserRoles { get; set; }
    }
}