using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static TestCase[] cases = new[]
        {
            new TestCase
            {
                Description = "IdentityServer4 Test Project",
                TokenEndpoint = "http://localhost:51571/connect/token",

                Apis =
                {
                    //new Api
                    //{
                    //    Description = "",
                    //    ClientId = "",
                    //    Url = "http://localhost:"
                    //},
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
                    var token = await GetToken(test.TokenEndpoint, api.ClientId, api.secret);

                    Console.WriteLine($"API:            {api.Description}");

                    await CallApi(api.Url, token);
                }

                Console.WriteLine("\n\n");
            }
            Console.ReadLine();
        }

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

        private static async Task CallApi(string api, string token)
        {
            var client = new HttpClient();
            client.SetBearerToken(token);

            var response = await client.GetStringAsync(api + "/ProtectedRessource");

            Console.WriteLine(response.ToString());
            Console.WriteLine("OK");
        }
    }

    public class TestCase
    {
        public string Description { get; set; }
        public string TokenEndpoint { get; set; }

        public ICollection<Api> Apis { get; set; } = new HashSet<Api>();
    }

    public class Api
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public string ClientId { get; set; }

        public string ID { get; set; }

        public string secret { get; set; }
    }
}
