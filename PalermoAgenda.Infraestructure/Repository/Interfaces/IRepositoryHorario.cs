
using PalermoAgenda.Infraestructure.Models;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryHorario
    {
        Task<ICollection<Horario>> ListAsync();
    }
}
