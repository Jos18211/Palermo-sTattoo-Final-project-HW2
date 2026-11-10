using System.ComponentModel.DataAnnotations;

namespace PalermoAgenda.Application.DTOs
{
    public record BloqueoDTO
    {
        public int IdBloqueo { get; set; }

        [Display(Name = "Sucursal")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una sucursal")]
        public int IdArtista { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "{0} es requerida")]
        public DateOnly Fecha { get; set; }

        // Si es todo el día no lleva horas (regla CK_Bloqueo_Tipo de la BD)
        [Display(Name = "Todo el día")]
        public bool TodoElDia { get; set; }

        [Display(Name = "Hora de inicio")]
        public TimeOnly? HoraInicio { get; set; }

        [Display(Name = "Hora de fin")]
        public TimeOnly? HoraFin { get; set; }

        [Display(Name = "Motivo")]
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "{0} debe tener entre 5 y 200 caracteres")]
        public string? Motivo { get; set; }

        // Solo para mostrar (listado y detalle)
        [Display(Name = "Sucursal")]
        public string? NombreSucursal { get; set; }
    }
}
