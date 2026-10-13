using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class Factura
{
    public int IdFactura { get; set; }

    public int IdCliente { get; set; }

    public int IdArtista { get; set; }

    public int? IdCita { get; set; }

    public DateTime FechaFactura { get; set; }

    public string Estado { get; set; } = null!;

    public decimal Subtotal { get; set; }

    public decimal PorcentajeImpuesto { get; set; }

    public decimal MontoImpuesto { get; set; }

    public decimal Total { get; set; }

    public virtual ICollection<FacturaDetalle> FacturaDetalle { get; set; } = new List<FacturaDetalle>();

    public virtual Artista IdArtistaNavigation { get; set; } = null!;

    public virtual Cita? IdCitaNavigation { get; set; }

    public virtual Usuario IdClienteNavigation { get; set; } = null!;
}
