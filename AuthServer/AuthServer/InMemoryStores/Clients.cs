using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.InMemoryStores
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                // Client mit OAuth 2.0 client credentials grant type
                // benötigt eine ClientId, ein ClientSecret
                // definiert Scopes für den Zugriff
                new Client
                {
                    ClientId = "IS3api",
                    ClientName = "Web API mit IdentityServer 3",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {
                        new Secret("superSecretPassword".Sha256())},
                    // Secret wird über eine von IS4 gebotene extension method gehasht
                    AllowedScopes = new List<string> {"customAPI.read"}
                    // Als Scope wird hier ein selbst implementiertes aus der Klasse Resources verwendet
                },

                new Client {
                    ClientId = "openIdConnectClient",
                    ClientName = "Example Implicit Client Application",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "customAPI"
                    },
                    RedirectUris = new List<string> {"https://localhost:44342/signin-oidc"},
                    PostLogoutRedirectUris = new List<string> { "https://localhost:44342/" }
                },

                new Client
                {
                    ClientId= "testApi",
                    ClientName = "Web API mit IdentityServer 4",
                    ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "customAPI.read"
                    }
                }
            };
      
        }

    }
}
