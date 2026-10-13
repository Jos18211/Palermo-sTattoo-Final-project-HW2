using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class Artista
{
    public int IdArtista { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Imagen { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Bloqueo> Bloqueo { get; set; } = new List<Bloqueo>();

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<Factura> Factura { get; set; } = new List<Factura>();

    public virtual ICollection<Horario> Horario { get; set; } = new List<Horario>();

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();

    public virtual ICollection<Servicio> IdServicio { get; set; } = new List<Servicio>();
}
