using Microsoft.AspNetCore.Mvc;
using PalermoAgenda.Application.Services.Interfaces;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class ServicioController : Controller
    {
        private readonly IServiceServicio _serviceServicio;
        public ServicioController(IServiceServicio serviceServicio)
        {
            _serviceServicio = serviceServicio;
        }

        // GET: Servicio
        public async Task<ActionResult> Index()
        {
            var collection =
                await _serviceServicio.ListAsync();
            return View(collection);
        }

        // GET: Servicio/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var @object = await _serviceServicio
                .FindByIdAsync(id.Value);
            if (@object == null)
            {
                throw new Exception("Servicio no existente");
            }
            return View(@object);
        }
    }
}
