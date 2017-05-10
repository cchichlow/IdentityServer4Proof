using System.Web.Http;

namespace WebAPIwithIS3
{
    // Das Attribut[Authorize] triggert die Autorisierung über den IdentityServer.
    /// <summary>
    /// Klasse die über den IdentityServer geschützt ist.
    /// Geschützte Ressource wird erreicht über die URI "../ProtectedRessource".
    /// </summary>
    [Route("ProtectedRessource")]
    [Authorize]
    public class TestController : ApiController
    {
        public string Get()
        {
            return "Protected Ressource reached!";
        }
    }
}
