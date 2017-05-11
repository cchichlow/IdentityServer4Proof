using System.Collections.Generic;
using IdentityServer4.Models;


namespace AuthServer.InMemoryStores
{
    public class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
                    new IdentityResources.OpenId(), //Wird immer für OpenID Connect Flows benötigt
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                    new IdentityResource {
                        Name = "role",
                        UserClaims = new List<string> {"role"}
                     }
                 };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                    new ApiResource {
                        Name = "customAPI",
                        DisplayName = "Custom API",
                        Description = "Custom API Access",
                        // Weil Claim "role" im Scope gesetzt ist, wird der Claim jedem Token hinzugefügt, der diesen Scope hat
                        UserClaims = new List<string> {"role"},
                        ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
                        Scopes = new List<Scope> {
                            new Scope("customAPI.read"),
                            new Scope("customAPI.write")
                        }
                    }
                 };
        }
    }
}
