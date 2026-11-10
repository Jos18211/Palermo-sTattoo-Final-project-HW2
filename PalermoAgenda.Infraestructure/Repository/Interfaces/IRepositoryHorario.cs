using PalermoAgenda.Infraestructure.Models;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryHorario
    {
        Task<ICollection<Horario>> ListAsync();
        Task<ICollection<Horario>> ListByArtistaAsync(int idArtista);
        Task<ICollection<Horario>> ListByArtistaFechaAsync(int idArtista, DateOnly fecha);
        Task<Horario?> FindByIdAsync(int id);
        Task<int> AddAsync(Horario entity);
    }
}
