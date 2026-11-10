
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.Services.Interfaces
{
    public interface IServiceArtista
    {
        Task<ICollection<ArtistaDTO>> ListAsync();
        Task<ArtistaDTO> FindByIdAsync(int id);
        Task<ICollection<ArtistaDTO>> FindByNameAsync(string nombre);
        Task<ICollection<ArtistaDTO>> ListAsyncAdmin();
        Task<int> AddAsync(ArtistaDTO entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id,ArtistaDTO entity);
        Task<bool> ExisteEmailAsync(string email, int idExcluir);
    }
}
