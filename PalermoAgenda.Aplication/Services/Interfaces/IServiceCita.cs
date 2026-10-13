using PalermoAgenda.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.Services.Interfaces
{
    public interface IServiceCita
    {
        Task<ICollection<CitaDTO>> ListByEncargadoAsync(int idEncargado);
        Task<CitaDTO> FindByIdAsync(int id, int idEncargado);
    }
}
