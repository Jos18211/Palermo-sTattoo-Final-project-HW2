using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IServiceProducto _serviceProducto;
        private readonly IServiceCategoria _serviceCategoria;
        private readonly ILogger<ProductoController> _logger;
        private readonly IWebHostEnvironment _env;
        public ProductoController(IServiceProducto servicioProducto, IWebHostEnvironment env, IServiceCategoria serviceCategoria,
            ILogger<ProductoController> logger)
        {
            _serviceProducto = servicioProducto;
            _serviceCategoria = serviceCategoria;
            _env = env;
            _logger = logger;
        }
        // GET: LibroController
        public async Task<ActionResult> Index()
        {
            //Recibir el mensaje de TempData
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            ViewBag.ListCategorias = await _serviceCategoria.ListAsync();
            var collection = await _serviceProducto.ListAsync();
            return View(collection);
        }
        public async Task<IActionResult> buscarxCategoria(int IdCategoria)
        {
            var collection = await _serviceProducto.ListAsync();
            if (IdCategoria != 0)
            {
                collection = await _serviceProducto.GetProductoByCategoria(IdCategoria) ?? new List<ProductoDTO>(); ;
            }
            return PartialView("_ListProductos", collection);
        }
        public async Task<ActionResult> IndexAdmin(int? page)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            var collection = await _serviceProducto.ListAsync();
            return View(collection.ToPagedList(page ?? 1, 5));
        }
        // GET: LibroController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                var @object = await _serviceProducto.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Producto no existente");
                }
                return View(@object);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // GET: LibroController/Create
        public async Task<IActionResult> Create()
        {

            var categorias = await _serviceCategoria.ListAsync();
            ViewBag.ListCategorias = new SelectList(
                 items: categorias,
                 dataValueField: nameof(CategoriaDTO.IdCategoriaProducto),
                 dataTextField: nameof(CategoriaDTO.Nombre));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoDTO dto, IFormFile? imageFile)
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
                    Path.Combine(_env.WebRootPath,"images","nophoto.png"));
                }
            }
            if (!ModelState.IsValid)
            {
                // Lee del ModelState todos los errores que
                // vienen para el lado del server
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Producto", errors, Util.SweetAlertMessageType.error);
                // Response errores
                var categorias = await _serviceCategoria.ListAsync();
                ViewBag.ListCategorias = new SelectList(
                     items: categorias,
                     dataValueField: nameof(CategoriaDTO.IdCategoriaProducto),
                     dataTextField: nameof(CategoriaDTO.Nombre));

                return View(dto);
            }
            await _serviceProducto.AddAsync(dto);
            //ViewBag.NotificationMessage= Util.SweetAlertHelper.Mensaje(
            //    "Crear Libro",
            //    "Libro creado", Util.SweetAlertMessageType.success);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Crear Producto",
                "Producto creado", Util.SweetAlertMessageType.success);
            return RedirectToAction("IndexAdmin");
        }
        // GET: LibroController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var @object = await _serviceProducto.FindByIdAsync(id);

            var categorias = await _serviceCategoria.ListAsync();
            if (@object == null) {
                return RedirectToAction("IndexAdmin");
            }
            else
            {
                
           
                ViewBag.ListCategorias = new SelectList(
                        items: categorias,
                        dataValueField: nameof(CategoriaDTO.IdCategoriaProducto),
                        dataTextField: nameof(CategoriaDTO.Nombre),
                        selectedValue: @object.IdCategoriaProducto
                    );
            return View(@object);
            }
        }
        // POST: LibroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoDTO dto, IFormFile? imageFile)
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
                    var actual = await _serviceProducto.FindByIdAsync(id);
                    dto.Imagen = actual.Imagen;
                }

            }

            if (!ModelState.IsValid)
            {
                // Lee del ModelState todos los errores que
                // vienen para el lado del server
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Producto", errors, Util.SweetAlertMessageType.error);


                var categorias = await _serviceCategoria.ListAsync();
                ViewBag.ListCategorias = new SelectList(
                    
                     items: categorias,
                     dataValueField: nameof(CategoriaDTO.IdCategoriaProducto),
                     dataTextField: nameof(CategoriaDTO.Nombre),
                     selectedValue: dto.IdCategoriaProducto);

                return View(dto);
            }
            else
            {
                await _serviceProducto.UpdateAsync(id, dto);
                TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Editar Producto",
                "Producto actualizado", Util.SweetAlertMessageType.success);
                return RedirectToAction("IndexAdmin");
            }
        }
        // GET: LibroController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var @object = await _serviceProducto.FindByIdAsync(id);
            return View(@object);
        }
        // POST: LibroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            await _serviceProducto.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GetProductoByName(string filtro)
        {
            var collection = await _serviceProducto.FindByNameAsync(filtro);
            return Json(collection.Select(p => new
            {
                p.IdProducto,
                p.Nombre,
                p.Precio,
                p.Stock,
                p.Imagen
            }));
        }
    }
}