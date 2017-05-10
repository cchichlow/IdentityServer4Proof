using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Logging;
using Owin;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;


namespace WebAPIwithIS3
{
    /// <summary>
    /// Einstiegspunkt für WebHostBuilder
    /// </summary>
    class StartUp
    {
        // Diese Methode wird vom Laufzeitsystem aufgerufen
        // Die Einbindung des IdentityServer läuft über die Owin Pipeline.
        /// <summary>
        /// Ergänzt die HTTP-Request-Pipeline um Coockie-Authentifizierung und OpenID-Authentifizierung.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            // Ausgabe auf der Konsole einbinden
            app.SetLoggerFactory(new ConsoleLoggerFactory());

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            // Im Produktiv-Einsatz fordert der IdentityServer eine SSL-Verschlüsselte Verbindung
            //var certPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "IdServv3.pfx");
            //var cert = (X509Certificate2)X509Certificate.CreateFromCertFile("D:\\WorkDir\\Git\\IdentityServer4Proof\\WebAPIwithIS3\\IdServv3.pfx");

            // IdentityServer 3 einbinden
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                // // Der Client muss zuvor mit seiner ID und seinem secret im AuthServer registriert sein
                ClientId = "IS3api",
                ClientSecret = "superSecretPassword",
                // Authority: Adresse des AuthServers
                Authority = "http://localhost:51571",
                // Für den Client erlaubte Scopes sind im AuthServer registriert
                RequiredScopes = new[] { "customAPI.read" },

                //SigningCertificate = cert,

                DelayLoadMetadata = true
            });

            app.UseWebApi(config);
        }
    }
}
