using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCita
    {
        Task<ICollection<Cita>> ListByEncargadoAsync(int idEncargado);
        Task<ICollection<Cita>> ListByArtistaAsync(int idArtista);

        Task<ICollection<Cita>> ListByArtistaFechaAsync(int idArtista, DateOnly fecha);
        Task<Cita> FindByIdAsync(int id, int idEncargado);
        Task<Cita> FindByIdChangeAsync(int id);
        Task<Cita> FindByIdAsync(int id);
        Task<int> AddAsync(Cita entity);
        Task CancelAsync(int id);
        Task UpdateAsync(Cita entity);



    }
}
