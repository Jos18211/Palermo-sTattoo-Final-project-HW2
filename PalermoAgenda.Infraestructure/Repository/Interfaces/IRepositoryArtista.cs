
using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryArtista
    {
        Task<ICollection<Artista>> ListAsync();
        Task<Artista> FindByIdAsync(int id);
        Task<ICollection<Artista>> FindByNameAsync(string nombre);
        Task<ICollection<Artista>> ListAsyncAdmin();
        Task<int> AddAsync(Artista entity, int[] encargados);
        Task DeleteAsync(int id);
        Task UpdateAsync(Artista entity,int[] encargados);
        Task<bool> ExisteEmailAsync(string email, int idExcluir);
      

    }
}
