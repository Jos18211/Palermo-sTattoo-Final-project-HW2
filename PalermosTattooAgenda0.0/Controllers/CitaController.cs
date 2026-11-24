using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PalermoAgenda.Application.Services.Implementations;
using PalermoAgenda.Application.Services.Interfaces;
using PalermoAgenda.Application.DTOs;
using PalermosTattooAgenda0.Web.Util;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class CitaController : Controller
    {
        private readonly IServiceCita _serviceCita;
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServiceArtista _serviceArtista;
        private readonly IServiceServicio _serviceServicio;
        private readonly IServiceHorario _serviceHorario;
        private readonly IServiceBloqueo _serviceBloqueo;
        // Se define en un solo lugar: Util/EncargadoActual.cs
        private const int IdEncargado = EncargadoActual.Id;
        public CitaController(IServiceCita serviceCita, IServiceUsuario serviceUsuario,
                              IServiceArtista serviceArtista, IServiceServicio serviceServicio,
                              IServiceHorario serviceHorario, IServiceBloqueo serviceBloqueo)
        {
            _serviceCita = serviceCita;
            _serviceUsuario = serviceUsuario;
            _serviceArtista = serviceArtista;
            _serviceServicio = serviceServicio;
            _serviceHorario = serviceHorario;
            _serviceBloqueo = serviceBloqueo;
        }
        public async Task<ActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.NotificationMessage = TempData["Mensaje"];
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

        // Datos que el formulario muestra pero no se editan: sucursal, servicios, horarios y bloqueos
        private async Task CargarDatos(CitaDTO dto)
        {
            var sucursal = await _serviceArtista.FindByIdAsync(dto.IdArtista);
            ViewBag.NombreSucursal = sucursal?.Nombre;
            ViewBag.Servicios = await _serviceServicio.ListByArtistaAsync(dto.IdArtista);
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            ViewBag.Horarios = (await _serviceHorario.ListByArtistaAsync(dto.IdArtista))
                                .Where(h => h.Fecha >= hoy).OrderBy(h => h.Fecha).ThenBy(h => h.HoraInicio).ToList();
            ViewBag.Bloqueos = (await _serviceBloqueo.ListByArtistaAsync(dto.IdArtista))
                                .Where(b => b.Fecha >= hoy).OrderBy(b => b.Fecha).ToList();
            // Si ya se había escogido un cliente (al volver con errores), se vuelve a mostrar
            if (dto.IdCliente > 0)
            {
                var cliente = await _serviceUsuario.FindByIdAsync(dto.IdCliente);
                ViewBag.ClienteNombre = cliente?.Nombre;
                ViewBag.ClienteEmail = cliente?.Email;
            }
        }

        // GET: Cita/Create
        public async Task<IActionResult> Create()
        {
            // La sucursal es la del encargado, no se selecciona
            var encargado = await _serviceUsuario.FindByIdAsync(IdEncargado);
            var dto = new CitaDTO
            {
                IdArtista = encargado.IdArtista ?? 0,
                Fecha = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                HoraInicio = new TimeOnly(10, 0)
            };
            await CargarDatos(dto);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaDTO dto)
        {
            // La sucursal siempre es la del encargado, no la que venga del formulario
            var encargado = await _serviceUsuario.FindByIdAsync(IdEncargado);
            dto.IdArtista = encargado.IdArtista ?? 0;

            // IdCliente exist? (si viene en 0 ya lo valida el [Range] del DTO)
            if (dto.IdCliente > 0 && await _serviceUsuario.FindByIdAsync(dto.IdCliente) == null)
                ModelState.AddModelError(string.Empty, "El cliente no existe");

            // Reglas de negocio: horario, bloqueos y sobreposición (capa de servicio)
            if (ModelState.IsValid)
            {
                var error = await _serviceCita.ValidarAsync(dto);
                if (error != null) ModelState.AddModelError(string.Empty, error);
            }

            // El formulario se envía por AJAX: los errores vuelven como BadRequest
            // y la vista los muestra con SweetAlert sin recargar la página
            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                return BadRequest(errors);
            }

            var idCita = await _serviceCita.AddAsync(dto);
            return Json(new { mensaje = $"Reserva #{idCita} registrada. Se generó su factura proforma." });
        }
    }
}
