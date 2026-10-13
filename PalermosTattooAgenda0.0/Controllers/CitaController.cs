using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PalermoAgenda.Application.Services.Implementations;
using PalermoAgenda.Application.Services.Interfaces;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class CitaController : Controller
    {
        private readonly IServiceCita _serviceCita;
        private const int IdEncargado = 2;
        public CitaController(IServiceCita serviceCita)
        {
            _serviceCita = serviceCita;
        }
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceCita.ListByEncargadoAsync(IdEncargado);
            return View(collection);
        }
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                var @object = await _serviceCita.FindByIdAsync(id.Value,IdEncargado);
                if (@object == null)
                {
                    throw new Exception("Cita no existente");
                }
                return View(@object);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
