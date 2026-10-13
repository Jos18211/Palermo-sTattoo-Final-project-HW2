using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.DTOs
{
    public record CitaDTO
    {
        public int IdCita { get; set; }

        public int IdCliente { get; set; }

        public int IdArtista { get; set; }

        public int IdServicio { get; set; }

        public int IdEstadoCita { get; set; }

        public DateOnly Fecha { get; set; }

        [Display(Name = "Hora de Inicio")]
        public TimeOnly HoraInicio { get; set; }

        [Display(Name = "Hora de finalización")]
        public TimeOnly HoraFin { get; set; }

        [Display(Name = "Zona del Cuerpo")]
        public string ZonaCuerpo { get; set; } = null!;

        [Display(Name = "¿Tiene alergias?")]
        public bool TieneAlergias { get; set; }

        [Display(Name = "Detalle de alergias")]
        public string? DetalleAlergias { get; set; }

        [Display(Name = "Referencia del diseño")]
        public string? ReferenciaDiseno { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Artista")]
        public virtual ArtistaDTO IdArtistaNavigation { get; set; } = null!;

        [Display(Name = "Cliente")]
        public virtual UsuarioDTO IdClienteNavigation { get; set; } = null!;

        [Display(Name = "Estado Cita")]
        public virtual EstadoCitaDTO IdEstadoCitaNavigation { get; set; } = null!;

        [Display(Name = "Servicio")]
        public virtual ServicioDTO IdServicioNavigation { get; set; } = null!;
    }
}
