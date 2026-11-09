using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Implementations;
using PalermoAgenda.Application.Services.Interfaces;
using X.PagedList.Extensions;


namespace PalermosTattooAgenda0.Web.Controllers
{
    public class ServicioController : Controller
    {
        private readonly IServiceServicio _serviceServicio;
        private readonly IWebHostEnvironment _env;
        public ServicioController(IServiceServicio serviceServicio, IWebHostEnvironment env)
        {
            _serviceServicio = serviceServicio;
            _env = env;
        }

        // GET: Servicio
        public async Task<ActionResult> Index()
        {
            var collection =
                await _serviceServicio.ListAsync();
            ViewBag.Estilos = collection.Select(s => s.Estilo).Distinct().OrderBy(e => e).ToList();
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
        private void CargarListas(string? estilo = null, string? complejidad = null)
        {
            var estilos = new[] { "Blackwork", "Fine line", "Realismo a color",
                          "Tradicional", "Personalizado", "Retoque" };
            var complejidades = new[] { "Baja", "Media", "Alta" };
            ViewBag.ListEstilos = new SelectList(estilos, estilo);
            ViewBag.ListComplejidad = new SelectList(complejidades, complejidad);
        }


        public async Task<IActionResult> buscarxEstilo(string estilo)
        {
            var collection = await _serviceServicio.ListAsync();
            if ( !string.IsNullOrEmpty(estilo))
            {
                collection = await _serviceServicio.GetServicioByEstilo(estilo) ?? new List<ServicioDTO>(); ;
            }
            return PartialView("_ListServicios", collection);
        }
        public async Task<ActionResult> IndexAdmin(int? page)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            var collection = await _serviceServicio.ListAsyncAdmin();
            return View(collection.ToPagedList(page ?? 1, 5));
        }
      
        // GET: LibroController/Create
        public async Task<IActionResult> Create()
        {
            CargarListas();

            return View(new ServicioDTO { Estado = true });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicioDTO dto, IFormFile? imageFile)
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
            if (!ModelState.IsValid)
            {
                // Lee del ModelState todos los errores que
                // vienen para el lado del server
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Servicio", errors, Util.SweetAlertMessageType.error);
                // Response errores

                CargarListas(dto.Estilo, dto.Complejidad);

                return View(dto);
            }
            await _serviceServicio.AddAsync(dto);
            //ViewBag.NotificationMessage= Util.SweetAlertHelper.Mensaje(
            //    "Crear Libro",
            //    "Libro creado", Util.SweetAlertMessageType.success);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Crear Servicio",
                "Servicio creado", Util.SweetAlertMessageType.success);
            return RedirectToAction("IndexAdmin");
        }
        // GET: LibroController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var @object = await _serviceServicio.FindByIdAsync(id);

     
            if (@object == null)
            {
                return RedirectToAction("IndexAdmin");
            }
            else
            {
               CargarListas(@object.Estilo, @object.Complejidad);

                return View(@object);
            }
        }
        // POST: LibroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServicioDTO dto, IFormFile? imageFile)
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
                    var actual = await _serviceServicio.FindByIdAsync(id);
                    dto.Imagen = actual?.Imagen ?? await System.IO.File.ReadAllBytesAsync(
                    Path.Combine(_env.WebRootPath, "images", "nophoto.png"));
                }

            }

            if (!ModelState.IsValid)
            {
                // Lee del ModelState todos los errores que
                // vienen para el lado del server
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Servicio", errors, Util.SweetAlertMessageType.error);

               CargarListas(dto.Estilo, dto.Complejidad);


                return View(dto);
            }
            else
            {
                await _serviceServicio.UpdateAsync(id, dto);
                TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Editar Servicio",
                "Servicio actualizado", Util.SweetAlertMessageType.success);
                return RedirectToAction("IndexAdmin");
            }
        }

        // POST: LibroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceServicio.DeleteAsync(id);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Eliminar Servicio",
                "Servicio desactivado", Util.SweetAlertMessageType.success);
            return RedirectToAction("IndexAdmin");
        }
        public async Task<IActionResult> GetServicioByName(string filtro)
        {
            var collection = await _serviceServicio.FindByNameAsync(filtro);
            return Json(collection.Select(p => new
            {
                p.IdServicio,
                p.Nombre,
                p.Precio,   
                p.Imagen
            }));
        }
    }
}
