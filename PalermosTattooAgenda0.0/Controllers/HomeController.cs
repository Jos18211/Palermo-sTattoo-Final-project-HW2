using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PalermosTattooAgenda0.Web.Models;
using System.Diagnostics;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        private List<string> ListarImagenes(string carpeta)
        {
            var ruta = Path.Combine(_env.WebRootPath, "images", "galeria", carpeta);
            if (!Directory.Exists(ruta))
            {
                return new List<string>();
            }
            return Directory.GetFiles(ruta)
                            .Select(f => Path.GetFileName(f))
                            .OrderBy(n => n)
                            .ToList();
        }

        public IActionResult Galeria()
        {
            ViewBag.Disenos = ListarImagenes("disenos");
            ViewBag.Trabajos = ListarImagenes("trabajos");
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult ErrorHandler(string messagesJson)
        {
            var errorMessages = JsonConvert.
                DeserializeObject<ErrorMiddlewareViewModel>(messagesJson);
            ViewBag.ErrorMessages = errorMessages;
            return View("ErrorHandler");
        }
    }
}
