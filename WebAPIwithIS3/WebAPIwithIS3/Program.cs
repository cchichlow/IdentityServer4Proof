using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Logging;
using Owin;
using System;
using System.Web.Http;

namespace WebAPIwithIS3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "WebAPI with IdentityServer 3";
           using (WebApp.Start<StartUp>("http://localhost:5072"))
            {
                Console.ReadLine();
            }
        }
    }
}
