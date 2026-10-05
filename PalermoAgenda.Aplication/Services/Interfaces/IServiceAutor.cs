
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.Services.Interfaces
{
    public interface IServiceAutor
    {
        Task<ICollection<AutorDTO>> ListAsync();
        Task<AutorDTO> FindByIdAsync(int id);
    }
}
