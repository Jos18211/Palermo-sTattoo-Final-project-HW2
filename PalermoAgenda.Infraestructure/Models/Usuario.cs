using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string PasswordHash { get; set; } = null!;

    public int IdRol { get; set; }

    public int? IdArtista { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string Identificacion { get; set; } = null!;

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<Factura> Factura { get; set; } = new List<Factura>();

    public virtual Artista? IdArtistaNavigation { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
