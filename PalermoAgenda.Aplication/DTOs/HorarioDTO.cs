namespace PalermoAgenda.Application.DTOs
{
    public record HorarioDTO
    {
        public int IdHorario { get; set; }
        public int IdArtista { get; set; }
        public byte DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}