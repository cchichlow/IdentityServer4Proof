using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AuthServer.Migrations;
using AuthServer.InMemoryStores;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.DbContexts;

namespace AuthServer
{
    public class Startup
    {
        // Diese Methode wird automatisch zur Laufzeit aufgerufen, bevor Configure-Methode aufgerufen wird.
        /// <summary>
        /// Ergänzt Service-Container um MVC-Service, Datenbankkontext, Identity-Kontext, IdentityServer.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Datenbankkontext definieren
            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=IdServ4;trusted_connection=yes;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
                        
            services.AddMvc();

            // Registration for ASP.NET Identity DbContext
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            // Fügt den IdentitServer zu den Services hinzu, womit er in der Anwendung über Dependency Injection verwendbar gemacht wird.
            services.AddIdentityServer()
                .AddOperationalStore(
                    builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(
                    builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                .AddAspNetIdentity<IdentityUser>()
                .AddTemporarySigningCredential();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Konsolenausgabe hinzufügen
            loggerFactory.AddConsole();

            app.UseDeveloperExceptionPage();

            // User, Clients und Ressourcen in DB migrieren, falls nicht vorhanden
            InitializeDbTestData(app);

            // Identity zur Pipeline hinzufügen, um User zu verwalten
            app.UseIdentity();
            // IdentityServer zur Pipeline hinzufügen, um Authentifizierungs- und Autorisierungsmechanismen zu integrieren
            app.UseIdentityServer();

            
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        /// <summary>
        /// Migriert Clients, User und Ressourcen in die lokale SQLite Datenbank.
        /// </summary>
        /// <param name="app"></param>
        private static void InitializeDbTestData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                // Client in DB migrieren
                if (!context.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                // Identity-Ressourcen in DB migrieren
                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in InMemoryStores.Resources.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                // Api-Ressourcen in DB migrieren
                if (!context.ApiResources.Any())
                {
                    foreach (var resource in InMemoryStores.Resources.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                // User werden über das Identity Framework verwaltet
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (!userManager.Users.Any())
                {
                    foreach (var testUser in AuthServer.InMemoryStores.TestUsers.Users)
                    {
                        var identityUser = new IdentityUser(testUser.Username)
                        {
                            Id = testUser.SubjectId
                        };

                        foreach (var claim in testUser.Claims)
                        {
                            identityUser.Claims.Add(new IdentityUserClaim<string>
                            {
                                UserId = identityUser.Id,
                                ClaimType = claim.Type,
                                ClaimValue = claim.Value,
                            });
                        }
                        userManager.CreateAsync(identityUser, testUser.Password).Wait();
                    }
                }
            }
        }
    }
}
