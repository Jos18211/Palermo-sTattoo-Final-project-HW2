
using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryServicio
    {

        Task<ICollection<Servicio>> FindByNameAsync(string nombre);
        Task<ICollection<Servicio>> ListAsync();
        Task<ICollection<Servicio>> ListAsyncAdmin();
        Task<Servicio?> FindByIdAsync(int id);
        Task<int> AddAsync(Servicio entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(Servicio entity);
        Task<ICollection<Servicio>> GetServicioByEstilo(string estilo);

    }
}


