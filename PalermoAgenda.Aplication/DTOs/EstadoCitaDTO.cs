using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.DTOs
{
    public record EstadoCitaDTO
    {
        public int IdEstadoCita { get; set; }

        public string Nombre { get; set; } = null!;

        public string Color { get; set; } = null!;
    }
}
