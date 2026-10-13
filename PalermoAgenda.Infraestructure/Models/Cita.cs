using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class Cita
{
    public int IdCita { get; set; }

    public int IdCliente { get; set; }

    public int IdArtista { get; set; }

    public int IdServicio { get; set; }

    public int IdEstadoCita { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public string ZonaCuerpo { get; set; } = null!;

    public bool TieneAlergias { get; set; }

    public string? DetalleAlergias { get; set; }

    public string? ReferenciaDiseno { get; set; }

    public bool Notificado { get; set; }

    public DateTime? FechaNotificacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Factura? Factura { get; set; }

    public virtual Artista IdArtistaNavigation { get; set; } = null!;

    public virtual Usuario IdClienteNavigation { get; set; } = null!;

    public virtual EstadoCita IdEstadoCitaNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;
}
