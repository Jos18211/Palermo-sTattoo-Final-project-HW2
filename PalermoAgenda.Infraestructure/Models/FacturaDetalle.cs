using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class FacturaDetalle
{
    public int IdFacturaDetalle { get; set; }

    public int IdFactura { get; set; }

    public int? IdProducto { get; set; }

    public int? IdServicio { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Factura IdFacturaNavigation { get; set; } = null!;

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }
}
