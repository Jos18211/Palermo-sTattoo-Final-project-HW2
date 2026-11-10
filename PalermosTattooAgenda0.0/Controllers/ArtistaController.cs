using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Implementations;
using PalermoAgenda.Application.Services.Interfaces;
using X.PagedList.Extensions;


namespace PalermosTattooAgenda0.Web.Controllers
{
    public class ArtistaController : Controller
    {
        private readonly IServiceArtista _serviceArtista;
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IWebHostEnvironment _env;

        public ArtistaController(IServiceArtista serviceArtista, IServiceUsuario serviceUsuario, IWebHostEnvironment env)
        {
            _serviceArtista = serviceArtista;
            _serviceUsuario = serviceUsuario;
            _env = env;
        }

        // Encargados que se pueden asignar: los libres y, al editar, los que ya tiene la sucursal
        private async Task CargarEncargados(int? idArtista, int[]? seleccionados)
        {
            var encargados = await _serviceUsuario.ListEncargadosDisponiblesAsync(idArtista);
            ViewBag.ListEncargados = new MultiSelectList(encargados, "IdUsuario", "Nombre", seleccionados);
        }

        // GET: ArtistaController
       
        public async Task<ActionResult> Index()
        {
            //Recibir Mensaje
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            ViewData["Title"] = "Artistas";
            ViewBag.Titulo = "Lista Artistas";
            var collection = await _serviceArtista.ListAsync();
            return View(collection);
        }

        // GET: ArtistaController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                var @object = await _serviceArtista.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Artista no existente");
                }
                return View(@object);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionResult> IndexAdmin(int? page)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            var collection = await _serviceArtista.ListAsyncAdmin();
            return View(collection.ToPagedList(page ?? 1, 5));
        }
        
        // GET: LibroController/Create
        public async Task<IActionResult> Create()
        {
            await CargarEncargados(null, null);
            return View(new ArtistaDTO { Estado = true });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArtistaDTO dto, IFormFile? imageFile)
        {
            MemoryStream target = new MemoryStream();
            // Cuando es Insert Image viene en null porque se pasa diferente
            if (dto.Imagen == null)
            {
                if (imageFile != null)
                {
                    imageFile.OpenReadStream().CopyTo(target);
                    dto.Imagen = target.ToArray();
                    ModelState.Remove("Imagen");
                }
                else
                {
                    dto.Imagen = await System.IO.File.ReadAllBytesAsync(
                    Path.Combine(_env.WebRootPath, "images", "nophoto.png"));
                }
            }
            if (!string.IsNullOrWhiteSpace(dto.Email) && await _serviceArtista.ExisteEmailAsync(dto.Email, 0))
            {
                ModelState.AddModelError("Email", "Ya existe una sucursal con ese email");
            }
            if (dto.SelectedEncargados == null || dto.SelectedEncargados.Length == 0)
            {
                ModelState.AddModelError("SelectedEncargados", "Seleccione al menos un encargado");
            }
            if (!ModelState.IsValid)
            {
                // Lee del ModelState todos los errores que
                // vienen para el lado del server
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Artista", errors, Util.SweetAlertMessageType.error);
                await CargarEncargados(null, dto.SelectedEncargados);
                return View(dto);
            }
            await _serviceArtista.AddAsync(dto);
            //ViewBag.NotificationMessage= Util.SweetAlertHelper.Mensaje(
            //    "Crear Libro",
            //    "Libro creado", Util.SweetAlertMessageType.success);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Crear Artista",
                "Artista creado", Util.SweetAlertMessageType.success);
            return RedirectToAction("IndexAdmin");
        }
        // GET: LibroController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var @object = await _serviceArtista.FindByIdAsync(id);
            if (@object == null)
            {
                return RedirectToAction("IndexAdmin");
            }
            // Preseleccionar los encargados actuales de la sucursal
            @object.SelectedEncargados = @object.Usuario?.Select(u => u.IdUsuario).ToArray() ?? Array.Empty<int>();
            await CargarEncargados(id, @object.SelectedEncargados);
            return View(@object);
        }
        // POST: LibroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArtistaDTO dto, IFormFile? imageFile)
        {


            MemoryStream target = new MemoryStream();
            // Cuando es Insert Image viene en null porque se pasa diferente
            if (dto.Imagen == null)
            {

                if (imageFile != null)
                {
                    imageFile.OpenReadStream().CopyTo(target);
                    dto.Imagen = target.ToArray();
                    ModelState.Remove("Imagen");
                }
                else
                {
                    var actual = await _serviceArtista.FindByIdAsync(id);
                    dto.Imagen = actual.Imagen;
                }

            }
            if (!string.IsNullOrWhiteSpace(dto.Email) && await _serviceArtista.ExisteEmailAsync(dto.Email, id))
            {
                ModelState.AddModelError("Email", "Ya existe una sucursal con ese email");
            }
            if (dto.SelectedEncargados == null || dto.SelectedEncargados.Length == 0)
            {
                ModelState.AddModelError("SelectedEncargados", "Seleccione al menos un encargado");
            }

            if (!ModelState.IsValid)
            {
                // Lee del ModelState todos los errores que
                // vienen para el lado del server
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Artista", errors, Util.SweetAlertMessageType.error);
                await CargarEncargados(id, dto.SelectedEncargados);
                return View(dto);
            }
            else
            {
                await _serviceArtista.UpdateAsync(id, dto);
                TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Editar Artista",
                "Artista actualizado", Util.SweetAlertMessageType.success);
                return RedirectToAction("IndexAdmin");
            }
        }

        // POST: LibroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceArtista.DeleteAsync(id);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Inavilitar Artista",
                "Artista desactivado", Util.SweetAlertMessageType.success);
            return RedirectToAction("IndexAdmin");
        }

    }
    }
