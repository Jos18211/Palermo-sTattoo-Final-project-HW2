using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalermoAgenda.Application.DTOs;


namespace PalermoAgenda.Application.Services.Interfaces
{
    public interface IServiceServicio
    {
        Task<ICollection<ServicioDTO>> FindByNameAsync(string nombre);
        Task<ICollection<ServicioDTO>> ListAsync();
        Task<ICollection<ServicioDTO>> ListAsyncAdmin();
        Task<ServicioDTO?> FindByIdAsync(int id);  
        Task<int> AddAsync(ServicioDTO dto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, ServicioDTO dto);
        Task<ICollection<ServicioDTO>> GetServicioByEstilo(string estilo);

    }
}
