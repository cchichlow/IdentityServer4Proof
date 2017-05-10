
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AuthServer.Migrations;
using AuthServer.InMemoryStores;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityModel;

namespace AuthServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Migriere Clients, User und Ressourcen in die lokale SQLite-DB
            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=IdServ4;trusted_connection=yes;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            
            services.AddMvc();

            // Registration for ASP.NET Identity DbContext
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddIdentityServer()
                .AddOperationalStore(
                    builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                .AddConfigurationStore(
                    builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                .AddAspNetIdentity<IdentityUser>()
                .AddTemporarySigningCredential();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseDeveloperExceptionPage();

            InitializeDbTestData(app);

            app.UseIdentity();
            app.UseIdentityServer();

            
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        private static void InitializeDbTestData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!context.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Resources.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Resources.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                if (!userManager.Users.Any())
                {
                    foreach (var testUser in AuthServer.TestUsers.Users)
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

                        userManager.CreateAsync(identityUser, "Password123!").Wait();
                    }
                }
            }
        }
    }

    internal class Resources
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



    internal class Users
    {
        public static List<IdentityServer4.Test.TestUser> Get()
        {
            return new List<IdentityServer4.Test.TestUser> {
                new IdentityServer4.Test.TestUser {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "scott",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                        new Claim(JwtClaimTypes.Role, "admin")
                    }
                }
            };

        }
    }
}
