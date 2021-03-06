﻿using IdentityModel.Client;
using Newtonsoft.Json.Linq;
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
                //Identity-Endpoint des AuthServers
                ClaimsEndpoint = "http://localhost:51571/identity",
                // WebAPI mit Angabem zum Client
                Apis =
                {
                    new Api
                    {
                        ClientId = "testApi",
                        Secret = "secret",
                        Description = "Web API on IS4",
                        UserName = "alice",
                        Url = "http://localhost:51124",
                        Password = "Password123!"
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
                    Console.WriteLine("Access-Token anfragen...");
                    var accessToken = await GetToken(test.TokenEndpoint, api.ClientId, api.Secret, api.UserName, api.Password);

                    Console.WriteLine($"API:            {api.Description}");
                    Console.WriteLine("Access-Token:" + Environment.NewLine +accessToken.ToString());

                    // Mit dem Access-Token die geschützte Ressource an der WebAPI anfragen
                    Console.WriteLine("Ressourcen anfragen...");
                    await CallApi(api.Url, accessToken);
                    await GetUserClaims(test.ClaimsEndpoint, accessToken);
                }

                Console.WriteLine(Environment.NewLine);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Token am Authorisierungsserver anfragen.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientID"></param>
        /// <param name="secret"></param>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        private static async Task<string> GetToken(string endpoint, string clientID, string secret, string userName, string userPassword)
        {
            var tokenClient = new TokenClient(endpoint, clientID, secret);
            var response = await tokenClient.RequestResourceOwnerPasswordAsync(userName: userName, password: userPassword, scope: "customAPI.read");

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }

            return response.AccessToken;
        }

        /// <summary>
        /// Die geschützte Api-Ressource (mit Access-Token) an der WebAPI anfragen.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private static async Task CallApi(string api, string accessToken)
        {
            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            Console.WriteLine("Api Ressource:");
            var response = await client.GetStringAsync(api + "/api/values/5");
            Console.WriteLine(response.ToString());
            
            Console.WriteLine("OK");
        }

        /// <summary>
        /// Die geschützte Identity-Ressource (mit Access-Token) an der WebAPI anfragen.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private static async Task GetUserClaims(string endpoint, string accessToken)
        {
            // call api
            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            Console.WriteLine("User Claims anfordern...");
            var response = await client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }

    /// <summary>
    /// Zu testendender Fall, mit Angabe zum Tokenendpunkt und der anzufragenden API.
    /// </summary>
    public class TestCase
    {
        public string Description { get; set; }
        public string TokenEndpoint { get; set; }
        public string ClaimsEndpoint { get; set; }

        public ICollection<Api> Apis { get; set; } = new HashSet<Api>();
    }

    /// <summary>
    /// Angaben zur anzufragenden API.
    /// </summary>
    public class Api
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string ClientId { get; set; }
        public string Secret { get; set; }
    }
}
