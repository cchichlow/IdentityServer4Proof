using System.Web.Http;

namespace WebAPIwithIS3
{
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
