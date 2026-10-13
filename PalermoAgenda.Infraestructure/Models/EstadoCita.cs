using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class EstadoCita
{
    public int IdEstadoCita { get; set; }

    public string Nombre { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
