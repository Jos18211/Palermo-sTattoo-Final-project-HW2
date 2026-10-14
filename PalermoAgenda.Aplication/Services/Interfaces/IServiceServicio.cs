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
        Task<ICollection<ServicioDTO>> ListAsync();
        Task<ServicioDTO?> FindByIdAsync(int id);
    }
}
