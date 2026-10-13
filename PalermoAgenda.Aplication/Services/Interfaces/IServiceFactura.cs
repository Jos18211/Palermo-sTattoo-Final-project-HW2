using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.Services.Interfaces
{
    public interface IServiceFactura
    {
        Task<ICollection<FacturaDTO>> ListAsync();
        Task<FacturaDTO> FindByIdAsync(int id);

        Task<FacturaDTO> FindByIdChangeAsync(int id);
        Task<int> AddAsync(FacturaDTO dto);
        Task<int> GetNextNumberFactura();
    }
}
