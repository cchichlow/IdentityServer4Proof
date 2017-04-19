using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Logging;
using Owin;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using Microsoft.Extensions.PlatformAbstractions;


namespace WebAPIwithIS3
{
    class StartUp
    {
        // Diese Methode wird vom Laufzeitsystem aufgerufen
        // Die Einbindung des IdentityServer läuft über die Owin Pipeline
        public void Configuration(IAppBuilder app)
        {
            // Ausgabe auf der Konsole einbinden
            app.SetLoggerFactory(new ConsoleLoggerFactory());

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            //var certPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "IdServv3.pfx");
            //var cert = (X509Certificate2)X509Certificate.CreateFromCertFile("D:\\WorkDir\\Git\\IdentityServer4Proof\\WebAPIwithIS3\\IdServv3.pfx");

            // IdentityServer 3 einbinden
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:51571",
                RequiredScopes = new[] { "customAPI.read" },
                //SigningCertificate = cert,
                ClientId = "IS3api",
                ClientSecret = "superSecretPassword",

                DelayLoadMetadata = true
            });

            app.UseWebApi(config);
        }
    }
}
