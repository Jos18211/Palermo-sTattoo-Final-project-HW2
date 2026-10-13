using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.Services.Interfaces
{
    public interface IServiceProducto
    {
        Task<ICollection<ProductoDTO>> FindByNameAsync(string nombre);
        Task<ICollection<ProductoDTO>> ListAsync();
        Task<ProductoDTO> FindByIdAsync(int id);
        Task<int> AddAsync(ProductoDTO dto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, ProductoDTO dto);

        Task<ICollection<ProductoDTO>> GetProductoByCategoria(int idCategoriaProducto);
    }
}
