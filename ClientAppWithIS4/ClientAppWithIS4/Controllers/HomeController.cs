using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClientAppWithIS4.Controllers
{
    /// <summary>
    /// Klasse zur Steuerung der verfügbaren Formulare
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        //Ressource, die nur authorisierten Benutzern vorenthalten ist
        // Das Attribut [Authorize] triggert die Autorisierung mittels IdentityServer
        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
