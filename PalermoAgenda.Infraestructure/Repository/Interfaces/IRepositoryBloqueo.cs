using PalermoAgenda.Infraestructure.Models;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryBloqueo
    {
        Task<ICollection<Bloqueo>> ListByArtistaAsync(int idArtista);
        Task<ICollection<Bloqueo>> ListByArtistaFechaAsync(int idArtista, DateOnly fecha);
        Task<Bloqueo?> FindByIdAsync(int id);
        Task<int> AddAsync(Bloqueo entity);
    }
}
