using System.ComponentModel.DataAnnotations;

namespace PalermoAgenda.Application.DTOs
{
    public record HorarioDTO
    {
        public int IdHorario { get; set; }

        [Display(Name = "Sucursal")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una sucursal")]
        public int IdArtista { get; set; }

        // Se calcula a partir de la fecha (1 = lunes ... 7 = domingo)
        [Display(Name = "Día")]
        public byte DiaSemana { get; set; }

        [Display(Name = "Hora de inicio")]
        [Required(ErrorMessage = "{0} es requerida")]
        public TimeOnly HoraInicio { get; set; }

        [Display(Name = "Hora de fin")]
        [Required(ErrorMessage = "{0} es requerida")]
        public TimeOnly HoraFin { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "{0} es requerida")]
        public DateOnly Fecha { get; set; }

        // Solo para mostrar (listado y detalle)
        [Display(Name = "Sucursal")]
        public string? NombreSucursal { get; set; }
    }
}
