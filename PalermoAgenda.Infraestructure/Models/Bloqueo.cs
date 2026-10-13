using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class Bloqueo
{
    public int IdBloqueo { get; set; }

    public int IdArtista { get; set; }

    public DateOnly Fecha { get; set; }

    public bool TodoElDia { get; set; }

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFin { get; set; }

    public string? Motivo { get; set; }

    public virtual Artista IdArtistaNavigation { get; set; } = null!;
}
