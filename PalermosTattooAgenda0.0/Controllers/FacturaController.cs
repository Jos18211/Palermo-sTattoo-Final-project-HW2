using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using System.Text.Json;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IServiceProducto _serviceProducto;
        private readonly IServiceFactura _serviceFactura;
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServiceArtista _serviceArtista;
        public FacturaController(IServiceProducto serviceProducto,
                             IServiceFactura serviceFactura,
                             IServiceUsuario serviceUsuario, IServiceArtista serviceArtista)
        {
            _serviceProducto = serviceProducto;
            _serviceFactura = serviceFactura;
            _serviceUsuario = serviceUsuario;
            _serviceArtista = serviceArtista;
        }
        // GET: FacturaController
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceFactura.ListAsync();
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
                var @object = await _serviceFactura.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Factura no existente");
                }
                return View(@object);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: FacturaController/Create
        public async Task<ActionResult> Create()
        {
            var nextReceiptNumber = await _serviceFactura.GetNextNumberFactura();
            ViewBag.CurrentReceipt = nextReceiptNumber;
            var artistas = await _serviceArtista.ListAsync();
            ViewBag.ListArtistas = new SelectList(items: artistas.Where(a => a.Estado),
                dataValueField: nameof(ArtistaDTO.IdArtista),
                dataTextField: nameof(ArtistaDTO.Nombre));
            // Clear CarShopping
            TempData["CartShopping"] = null;
            TempData.Keep();
            return View();
        }

        // POST: FacturaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FacturaDTO factura)
        {
            string json;
            try
            {
                // IdClient exist?
                var cliente = await _serviceUsuario.FindByIdAsync(factura.IdCliente);
                if (cliente == null)
                {
                    TempData.Keep();
                    return BadRequest("Cliente No existe");
                }
                if (factura.IdArtista <= 0)
                {
                    TempData.Keep();
                    return BadRequest("Seleccione la sucursal");
                }
                json = (string)TempData["CartShopping"]!;
                if (string.IsNullOrEmpty(json))
                {
                    TempData.Keep();
                    return BadRequest("No hay datos por facturar");
                }
                var lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
                //Agregar datos faltantes a la orden
                factura.IdFactura = 0;
                factura.FechaFactura = DateTime.Today;
                factura.Estado = "Firme";
                factura.FacturaDetalle = lista;
                factura.Subtotal = lista.Sum(p => p.Subtotal);
                factura.PorcentajeImpuesto = 13;
                factura.MontoImpuesto = factura.Subtotal * factura.PorcentajeImpuesto / 100;
                factura.Total = factura.Subtotal + factura.MontoImpuesto;
                
                await _serviceFactura.AddAsync(factura);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Keep Cache data
                TempData.Keep();
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> AddProducto(int id, int cantidad)
        {
            FacturaDetalleDTO detalle = new FacturaDetalleDTO();
            var lista = new List<FacturaDetalleDTO>();
            string json = "";
            
                var Producto = await _serviceProducto.FindByIdAsync(id);
            
            if (Producto == null || !Producto.Estado)
                {
                    TempData.Keep();
                    return BadRequest("El producto no está disponible");
                }
            FacturaDetalleDTO item = new FacturaDetalleDTO();
            //Cantidad de item a guardar
            detalle.Cantidad = cantidad;
            if (TempData["CartShopping"] != null)
            {
                json = (string)TempData["CartShopping"]!;
                lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
                //Buscar si existe en la compra
                item = lista.FirstOrDefault(o => o.IdProducto == id);
                if (item != null)
                {
                    detalle.Cantidad =item.Cantidad + cantidad;
                }
            }
            // Stock ??
            if (detalle.Cantidad > Producto.Stock)
            {
                TempData.Keep();
                return BadRequest("No hay inventario suficiente!");
            }
            else
            {
                if (item != null && item.Cantidad != 0)
                {
                    //Actualizar cantidad de libro existente
               
                    item.Cantidad += cantidad;
                    item.Subtotal = item.Cantidad * Producto.Precio;
                }
                else
                {
                    detalle.IdProducto = Producto.IdProducto;
                    detalle.NombreProducto=Producto.Nombre;
                    detalle.Cantidad = cantidad;
                    detalle.PrecioUnitario = Producto.Precio;
                    //Falta impuesto
                    detalle.Subtotal = (detalle.PrecioUnitario * detalle.Cantidad);
                    //Agregar al carrito de compras
                    lista.Add(detalle);
                }
                json = JsonSerializer.Serialize(lista);
                TempData["CartShopping"] = json;
            }
            TempData.Keep();
            return PartialView("_DetailFactura", lista);
        }
        public IActionResult GetDetailFactura()
        {
            List<FacturaDetalleDTO> lista = new List<FacturaDetalleDTO>();
            string json = "";
            json = (string)TempData["CartShopping"]!;
            lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
            json = JsonSerializer.Serialize(lista);
            TempData["CartShopping"] = json;
            TempData.Keep();
            return PartialView("_DetailFactura", lista);
        }



        // GET: FacturaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FacturaController/Edit/5
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

        // GET: FacturaController/Delete/5
        public IActionResult DeleteProducto(int idProducto)
        {
            FacturaDetalleDTO OrdenDetalleDTO = new FacturaDetalleDTO();
            List<FacturaDetalleDTO> lista = new List<FacturaDetalleDTO>();
            string json = "";
            if (TempData["CartShopping"] != null)
            {
                json = (string)TempData["CartShopping"]!;
                lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
                //Eliminar de la lista segun el indice
                int idx = lista.FindIndex(p => p.IdProducto == idProducto);
                if (idx >= 0) {
                lista.RemoveAt(idx);
                }
                
                json = JsonSerializer.Serialize(lista);
                TempData["CartShopping"] = json;
            }
            TempData.Keep();
            // return Content("Ok");
            return PartialView("_DetailFactura", lista);
        }
    


// POST: FacturaController/Delete/5
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
