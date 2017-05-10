using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestClient
{
    /// <summary>
    /// Testklasse simuliert einen Client, der Zugriff auf eine geschützte Ressource der WabAPI anfordert.
    /// </summary>
    class Program
    {
        static TestCase[] cases = new[]
        {
            new TestCase
            {
                Description = "IdentityServer4 Test Project",
                //Token-Endpoint des AuthServers
                TokenEndpoint = "http://localhost:51571/connect/token",
                // WebAPI mit Angabem zum Client
                Apis =
                {
                    new Api
                    {
                        Description = "Web API on IS3",
                        ClientId = "IS3api",
                        Url = "http://localhost:5072",
                        secret = "superSecretPassword"
                    }
                }
            }
        };

        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            Console.Title = "Test Client";
            Console.WriteLine("press enter to start...\n\n");
            Console.ReadLine();

            foreach (var test in cases)
            {
                Console.WriteLine($"Test:           {test.Description}\n");

                foreach (var api in test.Apis)
                {
                    // Access-Token am Autorisisierungsserver anfragen
                    var token = await GetToken(test.TokenEndpoint, api.ClientId, api.secret);

                    Console.WriteLine($"API:            {api.Description}");
                    // Mit dem Access-Token die geschützte Ressource an der WebAPI anfragen
                    await CallApi(api.Url, token);
                }

                Console.WriteLine("\n\n");
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Token am Authorisierungsserver anfragen.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientId"></param>
        /// <param name="apiSecret"></param>
        /// <returns></returns>
        private static async Task<string> GetToken(string endpoint, string clientId, string apiSecret)
        {
            var tokenClient = new TokenClient(endpoint, clientId, apiSecret);
            var response = await tokenClient.RequestClientCredentialsAsync("customAPI.read");

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }

            return response.AccessToken;
        }

        /// <summary>
        /// Die geschützte Ressource (mit Access-Token) an der WebAPI anfragen.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task CallApi(string api, string token)
        {
            var client = new HttpClient();
            client.SetBearerToken(token);

            var response = await client.GetStringAsync(api + "/ProtectedRessource");

            Console.WriteLine(response.ToString());
            Console.WriteLine("OK");
        }
    }

    /// <summary>
    /// Zu testendender Fall, mit Angabe zum Tokenendpunkt und der anzufragenden API.
    /// </summary>
    public class TestCase
    {
        public string Description { get; set; }
        public string TokenEndpoint { get; set; }

        public ICollection<Api> Apis { get; set; } = new HashSet<Api>();
    }

    /// <summary>
    ///  Angaben zur anzufragenden API.
    /// </summary>
    public class Api
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public string ClientId { get; set; }

        public string ID { get; set; }

        public string secret { get; set; }
    }
}
