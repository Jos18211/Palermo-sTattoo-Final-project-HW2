using PalermoAgenda.Application.DTOs;

namespace PalermoAgenda.Application.Services.Interfaces
{
    public interface IServiceHorario
    {
        Task<ICollection<HorarioDTO>> ListHorarioEstudioAsync();
        Task<ICollection<HorarioDTO>> ListByArtistaAsync(int idArtista);
        Task<HorarioDTO?> FindByIdAsync(int id);
        // Devuelve el mensaje de error o null si el horario es válido
        Task<string?> ValidarAsync(HorarioDTO dto);
        Task<int> AddAsync(HorarioDTO dto);
    }
}
