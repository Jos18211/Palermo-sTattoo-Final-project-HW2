using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public int IdArtista { get; set; }

    public byte DiaSemana { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public virtual Artista IdArtistaNavigation { get; set; } = null!;
}
