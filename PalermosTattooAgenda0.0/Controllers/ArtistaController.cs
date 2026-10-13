using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PalermoAgenda.Application.Services.Implementations;
using PalermoAgenda.Application.Services.Interfaces;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class ArtistaController : Controller
    {
        private readonly IServiceArtista _serviceArtista;

        public ArtistaController(IServiceArtista serviceArtista)
        {
            _serviceArtista = serviceArtista;
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

        public ActionResult Enviar()
        {
            //Procesamiento
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje("Enviar", "Mensaje enviado", Util.SweetAlertMessageType.success);
            //return View();
            return RedirectToAction("Index");
        }

        // GET: ArtistaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
    }
}