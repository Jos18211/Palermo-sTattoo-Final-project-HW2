using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PalermosTattooAgenda0.Web.Models;
using System.Diagnostics;
using PalermoAgenda.Application.Services.Interfaces;
using PalermosTattooAgenda0.Web.Services;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IServiceArtista _serviceArtista;
        private readonly IConfiguration _configuration;
        private readonly IServiceInstagram _serviceInstagram;
        private readonly IServiceHorario _serviceHorario;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env
                              , IServiceArtista serviceArtista, IConfiguration configuration, IServiceInstagram serviceInstagram, IServiceHorario serviceHorario)
        {
            _logger = logger;
            _env = env;
            _configuration = configuration;
            _serviceArtista = serviceArtista;
            _serviceInstagram = serviceInstagram;
            _serviceHorario = serviceHorario;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Horario =
                await _serviceHorario.ListHorarioEstudioAsync();
            ViewBag.Artistas =
                await _serviceArtista.ListAsync();
            ViewBag.Trabajos = ListarImagenes("trabajos")
                .Take(4).ToList();
            ViewBag.MapsKey = _configuration["GoogleMaps:ApiKey"];
            ViewBag.Instagram =await _serviceInstagram.ListAsync(4);
            
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
