using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PalermoAgenda.Application.DTOs
{
    public record HorarioDTO
    {
        public int IdHorario { get; set; }
        [Display(Name = "Sucursal")]
        public int IdArtista { get; set; }
        [Display(Name = "Día")]
        public byte DiaSemana { get; set; }
        [Display(Name = "Hora de inicio")]
        public TimeOnly HoraInicio { get; set; }
        [Display(Name = "Hora de fin")]
        public TimeOnly HoraFin { get; set; }

        [Display(Name = "Fecha")]
        public DateOnly Fecha { get; set; }

    }
}