using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ClientAppWithIS4
{
    /// <summary>
    /// Einstiegspunkt des Programms. Automatisch vpn VS2017 generierte, nicht veränderte Klasse.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
