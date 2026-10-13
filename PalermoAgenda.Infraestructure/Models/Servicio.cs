using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int DuracionMinutos { get; set; }

    public string Estilo { get; set; } = null!;

    public string Complejidad { get; set; } = null!;

    public bool Estado { get; set; }

    public byte[]? Imagen { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<FacturaDetalle> FacturaDetalle { get; set; } = new List<FacturaDetalle>();

    public virtual ICollection<Artista> IdArtista { get; set; } = new List<Artista>();
}
