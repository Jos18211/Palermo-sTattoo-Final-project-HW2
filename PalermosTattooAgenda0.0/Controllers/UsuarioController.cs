using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PalermoAgenda.Application.Services.Implementations;
using PalermoAgenda.Application.Services.Interfaces;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;

        public UsuarioController(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }

        // GET: ArtistaController

        public async Task<ActionResult> Index()
        {
            //Recibir Mensaje
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            ViewData["Title"] = "Usuario";
            ViewBag.Titulo = "Lista Usuarios";
            var collection = await _serviceUsuario.ListAsync();
            return View(collection);
        }

        public ActionResult Enviar()
        {
            //Procesamiento
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje("Enviar", "Mensaje enviado", Util.SweetAlertMessageType.success);
            //return View();
            return RedirectToAction("Index");
        }

        // GET: ArtistaController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var @object = await _serviceUsuario.FindByIdAsync(id.Value);
            if (@object == null)
            {
                throw new Exception("Usuario no existente");
            }
            return View(@object);
        }

        // GET: ArtistaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtistaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArtistaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ArtistaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArtistaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ArtistaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<IActionResult> GetClienteByName(string filtro)
        {
            var collection = await _serviceUsuario.FindByNombreRolAsync(filtro);
            return Json(collection.Select(u => new { u.IdUsuario, u.Nombre, u.Identificacion }));
        }


    }
}