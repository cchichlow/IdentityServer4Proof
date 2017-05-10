using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace ClientAppWithIS4
{
    /// <summary>
    /// Einstiegspunkt für WebHostBuilder
    /// </summary>
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

        // Diese Methode wird automatisch zur Laufzeit aufgerufen. Sie kann verwendet werden, um die HTTP-Request-Pipeline zu konfigurieren.
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Cookie-Authentifizierung um den Client-Informationen zu speichern
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "cookie"
            });

            // OpenID-Authentifizierung auf Basis der zuvor eingebundenen Coockie-Middleware
            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                // Der Client muss zuvor mit seiner ID im AuthServer registriert sein
                ClientId = "openIdConnectClient",
                // Authority: Adresse des AuthServers
                Authority = "https://localhost:44399/",
                // Die zuvor definierte Cookie-Middleware soll verwendet werden,
                // um die Identität des Benutzers zu speichern sobald die Authentifizerung erfolgreich war
                SignInScheme = "cookie"
            });

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


                       
           
        }
    }
}
