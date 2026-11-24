using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using PalermosTattooAgenda0.Web.Util;
using System.Text.Json;

namespace PalermosTattooAgenda0.Web.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IServiceProducto _serviceProducto;
        private readonly IServiceFactura _serviceFactura;
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServiceArtista _serviceArtista;
        private readonly IServiceServicio _serviceServicio;
        // El encargado se establece en código, no en la interfaz (Util/EncargadoActual.cs)
        private const int IdEncargado = EncargadoActual.Id;
        public FacturaController(IServiceProducto serviceProducto,
                             IServiceFactura serviceFactura,
                             IServiceUsuario serviceUsuario, IServiceArtista serviceArtista,
                             IServiceServicio serviceServicio)
        {
            _serviceServicio = serviceServicio;
            _serviceProducto = serviceProducto;
            _serviceFactura = serviceFactura;
            _serviceUsuario = serviceUsuario;
            _serviceArtista = serviceArtista;
        }
        // GET: FacturaController
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceFactura.ListByEncargadoAsync(IdEncargado);
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
                // Solo se muestran las facturas de la sucursal del encargado (también por URL)
                var encargado = await _serviceUsuario.FindByIdAsync(IdEncargado);
                if (@object == null || @object.IdArtista != encargado.IdArtista)
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
            // Sucursal y encargado: se muestran, no se seleccionan
            var encargado = await _serviceUsuario.FindByIdAsync(IdEncargado);
            await CargarEncabezado(encargado);
            // Clear CarShopping
            TempData["CartShopping"] = null;
            TempData.Keep();
            return View(new FacturaDTO { IdArtista = encargado.IdArtista ?? 0 });
        }

        private async Task CargarEncabezado(UsuarioDTO encargado)
        {
            var sucursal = await _serviceArtista.FindByIdAsync(encargado.IdArtista ?? 0);
            ViewBag.NombreEncargado = encargado.Nombre;
            ViewBag.NombreSucursal = sucursal?.Nombre;
            // Servicios que ofrece la sucursal, para agregarlos al detalle
            ViewBag.Servicios = await _serviceServicio.ListByArtistaAsync(encargado.IdArtista ?? 0);
        }

        // GET: FacturaController/Facturar/5 -> precarga una proforma para facturarla
        public async Task<ActionResult> Facturar(int id)
        {
            var encargado = await _serviceUsuario.FindByIdAsync(IdEncargado);
            var proforma = await _serviceFactura.FindByIdAsync(id);
            if (proforma == null || proforma.Estado != "Proforma" || proforma.IdArtista != encargado.IdArtista)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentReceipt = proforma.IdFactura;
            ViewBag.Proforma = true;
            await CargarEncabezado(encargado);
            // El detalle de la proforma pasa al carrito para seguir editándolo
            var lista = proforma.FacturaDetalle.Select(d => new FacturaDetalleDTO
            {
                IdProducto = d.IdProducto,
                NombreProducto = d.IdProductoNavigation?.Nombre,
                IdServicio = d.IdServicio,
                NombreServicio = d.IdServicioNavigation?.Nombre,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.PrecioUnitario * d.Cantidad
            }).ToList();
            TempData["CartShopping"] = JsonSerializer.Serialize(lista);
            TempData.Keep();
            proforma.FacturaDetalle = lista;
            return View("Create", proforma);
        }

        // POST: FacturaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FacturaDTO factura, int? idProforma)
        {
            string json;
            try
            {
                // IdClient exist?
                var cliente = await _serviceUsuario.FindByIdAsync(factura.IdCliente);
                if (cliente == null)
                {
                    TempData.Keep();
                    return BadRequest("Cliente no existe");
                }
                // La sucursal siempre es la del encargado
                var encargado = await _serviceUsuario.FindByIdAsync(IdEncargado);
                factura.IdArtista = encargado.IdArtista ?? 0;
                if (factura.IdArtista <= 0)
                {
                    TempData.Keep();
                    return BadRequest("El encargado no tiene artista asignado");
                }
                json = (string)TempData["CartShopping"]!;
                if (string.IsNullOrEmpty(json))
                {
                    TempData.Keep();
                    return BadRequest("No hay datos por facturar");
                }
                var lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
                if (lista.Count == 0)
                {
                    TempData.Keep();
                    return BadRequest("No hay datos por facturar");
                }
                //Agregar datos faltantes a la orden
                factura.IdFactura = 0;
                factura.FechaFactura = DateTime.Today;
                factura.Estado = "Firme";
                factura.FacturaDetalle = lista;
                factura.Subtotal = lista.Sum(p => p.Subtotal);
                factura.PorcentajeImpuesto = 13;
                factura.MontoImpuesto = lista.Sum(p => p.Impuesto);
                factura.Total = factura.Subtotal + factura.MontoImpuesto;

                if (idProforma.HasValue)
                {
                    // Facturar proforma: misma fila, el cliente debe ser el de la proforma
                    var proforma = await _serviceFactura.FindByIdChangeAsync(idProforma.Value);
                    if (proforma == null || proforma.Estado != "Proforma" || proforma.IdArtista != factura.IdArtista)
                    {
                        TempData.Keep();
                        return BadRequest("La proforma no existe o ya fue facturada");
                    }
                    if (proforma.IdCliente != factura.IdCliente)
                    {
                        TempData.Keep();
                        return BadRequest("El cliente no corresponde al de la proforma");
                    }
                    factura.IdFactura = proforma.IdFactura;
                    factura.IdCita = proforma.IdCita;
                    await _serviceFactura.FacturarProformaAsync(factura);
                    return RedirectToAction("Index");
                }

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
                return BadRequest("¡No hay inventario suficiente!");
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

        public async Task<IActionResult> AddServicio(int id, int cantidad)
        {
            var lista = new List<FacturaDetalleDTO>();
            string json = "";
            if (cantidad <= 0)
            {
                TempData.Keep();
                return BadRequest("La cantidad debe ser mayor a cero");
            }
            // Solo servicios que ofrece la sucursal del encargado
            var encargado = await _serviceUsuario.FindByIdAsync(IdEncargado);
            var servicios = await _serviceServicio.ListByArtistaAsync(encargado.IdArtista ?? 0);
            var servicio = servicios.FirstOrDefault(s => s.IdServicio == id);
            if (servicio == null)
            {
                TempData.Keep();
                return BadRequest("El servicio no está disponible con este artista");
            }
            if (TempData["CartShopping"] != null)
            {
                json = (string)TempData["CartShopping"]!;
                lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
            }
            //Buscar si ya existe en la factura
            var item = lista.FirstOrDefault(o => o.IdServicio == id);
            if (item != null)
            {
                item.Cantidad += cantidad;
                item.Subtotal = item.Cantidad * item.PrecioUnitario;
            }
            else
            {
                lista.Add(new FacturaDetalleDTO
                {
                    IdServicio = servicio.IdServicio,
                    NombreServicio = servicio.Nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = servicio.Precio,
                    Subtotal = servicio.Precio * cantidad
                });
            }
            TempData["CartShopping"] = JsonSerializer.Serialize(lista);
            TempData.Keep();
            return PartialView("_DetailFactura", lista);
        }

        public IActionResult DeleteServicio(int idServicio)
        {
            List<FacturaDetalleDTO> lista = new List<FacturaDetalleDTO>();
            string json = "";
            if (TempData["CartShopping"] != null)
            {
                json = (string)TempData["CartShopping"]!;
                lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
                int idx = lista.FindIndex(p => p.IdServicio == idServicio);
                if (idx >= 0)
                {
                    lista.RemoveAt(idx);
                }
                json = JsonSerializer.Serialize(lista);
                TempData["CartShopping"] = json;
            }
            TempData.Keep();
            return PartialView("_DetailFactura", lista);
        }

        // Cambia la cantidad de una línea (producto o servicio); en 0 la línea se elimina
        public async Task<IActionResult> CambiarCantidad(int? idProducto, int? idServicio, int cantidad)
        {
            List<FacturaDetalleDTO> lista = new List<FacturaDetalleDTO>();
            if (cantidad < 0)
            {
                TempData.Keep();
                return BadRequest("La cantidad no puede ser negativa");
            }
            if (TempData["CartShopping"] != null)
            {
                string json = (string)TempData["CartShopping"]!;
                lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
            }
            var item = idProducto != null
                ? lista.FirstOrDefault(o => o.IdProducto == idProducto)
                : lista.FirstOrDefault(o => o.IdServicio == idServicio);
            if (item != null)
            {
                if (cantidad == 0)
                {
                    lista.Remove(item);
                }
                else
                {
                    if (item.IdProducto != null)
                    {
                        var producto = await _serviceProducto.FindByIdAsync(item.IdProducto.Value);
                        if (producto == null || cantidad > producto.Stock)
                        {
                            TempData.Keep();
                            return BadRequest("¡No hay inventario suficiente!");
                        }
                    }
                    item.Cantidad = cantidad;
                    item.Subtotal = item.Cantidad * item.PrecioUnitario;
                }
            }
            TempData["CartShopping"] = JsonSerializer.Serialize(lista);
            TempData.Keep();
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
