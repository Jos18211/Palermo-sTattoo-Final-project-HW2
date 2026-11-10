using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using PalermosTattooAgenda0.Web.Util;

namespace PalermosTattooAgenda0.Web.Controllers
{
    // Mantenimiento de Horarios y Bloqueos por sucursal
    public class HorarioController : Controller
    {
        private readonly IServiceHorario _serviceHorario;
        private readonly IServiceBloqueo _serviceBloqueo;
        private readonly IServiceArtista _serviceArtista;

        public HorarioController(IServiceHorario serviceHorario,
                                 IServiceBloqueo serviceBloqueo,
                                 IServiceArtista serviceArtista)
        {
            _serviceHorario = serviceHorario;
            _serviceBloqueo = serviceBloqueo;
            _serviceArtista = serviceArtista;
        }

        private async Task CargarSucursales(int? seleccionada)
        {
            var sucursales = await _serviceArtista.ListAsync();
            ViewBag.ListSucursales = new SelectList(sucursales,
                nameof(ArtistaDTO.IdArtista), nameof(ArtistaDTO.Nombre), seleccionada);
        }

        // GET: Horario?idArtista=1  -> horarios y bloqueos de la sucursal seleccionada
        public async Task<IActionResult> Index(int? idArtista)
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.NotificationMessage = TempData["Mensaje"];

            await CargarSucursales(idArtista);
            ViewBag.IdArtista = idArtista;
            ViewBag.Horarios = new List<HorarioDTO>();
            ViewBag.Bloqueos = new List<BloqueoDTO>();

            if (idArtista.HasValue)
            {
                ViewBag.Horarios = await _serviceHorario.ListByArtistaAsync(idArtista.Value);
                ViewBag.Bloqueos = await _serviceBloqueo.ListByArtistaAsync(idArtista.Value);
            }
            return View();
        }

        // GET: Horario/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _serviceHorario.FindByIdAsync(id);
            if (dto == null) return RedirectToAction(nameof(Index));
            return View(dto);
        }

        // GET: Horario/DetailsBloqueo/5
        public async Task<IActionResult> DetailsBloqueo(int id)
        {
            var dto = await _serviceBloqueo.FindByIdAsync(id);
            if (dto == null) return RedirectToAction(nameof(Index));
            return View(dto);
        }

        // GET: Horario/Create?idArtista=1
        public async Task<IActionResult> Create(int? idArtista)
        {
            await CargarSucursales(idArtista);
            return View(new HorarioDTO
            {
                IdArtista = idArtista ?? 0,
                Fecha = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                HoraInicio = new TimeOnly(9, 0),
                HoraFin = new TimeOnly(17, 0)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HorarioDTO dto)
        {
            // Reglas de negocio: fecha vigente y sobreposiciones (capa de servicio)
            if (ModelState.IsValid)
            {
                var error = await _serviceHorario.ValidarAsync(dto);
                if (error != null) ModelState.AddModelError(string.Empty, error);
            }

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Horario", errors, SweetAlertMessageType.error);
                await CargarSucursales(dto.IdArtista);
                return View(dto);
            }

            await _serviceHorario.AddAsync(dto);
            TempData["Mensaje"] = SweetAlertHelper.Mensaje("Crear Horario", "Horario registrado", SweetAlertMessageType.success);
            return RedirectToAction(nameof(Index), new { idArtista = dto.IdArtista });
        }

        // GET: Horario/CreateBloqueo?idArtista=1
        public async Task<IActionResult> CreateBloqueo(int? idArtista)
        {
            await CargarSucursales(idArtista);
            return View(new BloqueoDTO
            {
                IdArtista = idArtista ?? 0,
                Fecha = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                HoraInicio = new TimeOnly(12, 0),
                HoraFin = new TimeOnly(13, 0)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBloqueo(BloqueoDTO dto)
        {
            if (ModelState.IsValid)
            {
                var error = await _serviceBloqueo.ValidarAsync(dto);
                if (error != null) ModelState.AddModelError(string.Empty, error);
            }

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Bloqueo", errors, SweetAlertMessageType.error);
                await CargarSucursales(dto.IdArtista);
                return View(dto);
            }

            await _serviceBloqueo.AddAsync(dto);
            TempData["Mensaje"] = SweetAlertHelper.Mensaje("Crear Bloqueo", "Bloqueo registrado", SweetAlertMessageType.success);
            return RedirectToAction(nameof(Index), new { idArtista = dto.IdArtista });
        }
    }
}
