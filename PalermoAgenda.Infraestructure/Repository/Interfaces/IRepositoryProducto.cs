
using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryProducto
    {
        Task<ICollection<Producto>> FindByNameAsync(string nombre);
        Task<ICollection<Producto>> ListAsync();
        Task<Producto> FindByIdAsync(int id);
        Task<int> AddAsync(Producto entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(Producto entity);
        Task<ICollection<Producto>> GetProductoByCategoria(int idCategoriaProducto);
    }
}
