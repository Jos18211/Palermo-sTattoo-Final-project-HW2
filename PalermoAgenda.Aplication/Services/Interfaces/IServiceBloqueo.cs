using PalermoAgenda.Application.DTOs;

namespace PalermoAgenda.Application.Services.Interfaces
{
    public interface IServiceBloqueo
    {
        Task<ICollection<BloqueoDTO>> ListByArtistaAsync(int idArtista);
        Task<BloqueoDTO?> FindByIdAsync(int id);
        // Devuelve el mensaje de error o null si el bloqueo es válido
        Task<string?> ValidarAsync(BloqueoDTO dto);
        Task<int> AddAsync(BloqueoDTO dto);
    }
}
