using PalermoAgenda.Application.Validations;
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
        [Display(Name = "Identificador de la cita")]
        public int IdCita { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un Cliente")]
        public int IdCliente { get; set; }

        [Display(Name = "Artista")]
        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una sucursal")]
        public int IdArtista { get; set; }

        [Display(Name = "Servicio")]
        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un servicio")]
        public int IdServicio { get; set; }

        public int IdEstadoCita { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "{0} es requerida")]
        [CustomDateValidation(ErrorMessage = "El campo {0} debe ser una fecha mayor a la actual.")]
        public DateOnly Fecha { get; set; }

        [Display(Name = "Hora de inicio")]
        [Required(ErrorMessage = "{0} es requerida")]
        public TimeOnly HoraInicio { get; set; }

        [Display(Name = "Hora de finalización")]
        [Required(ErrorMessage = "{0} es requerida")]
        public TimeOnly HoraFin { get; set; }

        [Display(Name = "Zona del Cuerpo")]
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(100, MinimumLength = 3)]
        public string ZonaCuerpo { get; set; } = null!;

        [Display(Name = "¿Tiene alergias?")]
        public bool TieneAlergias { get; set; }

        [Display(Name = "Detalle de alergias")]
        [StringLength(250, MinimumLength = 3)]
        public string? DetalleAlergias { get; set; }

        [Display(Name = "Referencia del diseño")]
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(500, MinimumLength = 3)]
        public string? ReferenciaDiseno { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Artista")]
        public virtual ArtistaDTO? IdArtistaNavigation { get; set; } = null!;

        [Display(Name = "Cliente")]
        public virtual UsuarioDTO? IdClienteNavigation { get; set; } = null!;

        [Display(Name = "Estado Cita")]
        public virtual EstadoCitaDTO? IdEstadoCitaNavigation { get; set; } = null!;

        [Display(Name = "Servicio")]
        public virtual ServicioDTO? IdServicioNavigation { get; set; } = null!;


    }
}
