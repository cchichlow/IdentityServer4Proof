using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace OpenIdTestWebAPI
{
    public class Startup
    {
        /// <summary>
        /// Wurzelobjekt zum Speichern von Konfigurationen im Sinne
        /// von Umgebungsveriablen nach der Spezifikation der OWIN Middleware.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // Diese Methode wird automatisch zur Laufzeit aufgerufen, bevor Configure-Methode aufgerufen wird.
        /// <summary>
        /// Fügt einen MVC-Service dem Service-Container hinzu.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // Diese Methode wird vom Laufzeitsystem aufgerufen
        // Die Einbindung des IdentityServer läuft über die Owin Pipeline.
        /// <summary>
        /// Ergänzt die HTTP-Request-Pipeline um Coockie-Authentifizierung und OpenID-Authentifizierung.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:51571/",
                AllowedScopes = new List<string> { "customApi"},
                RequireHttpsMetadata = false
            });

            app.UseMvc();
        }
    }
}
