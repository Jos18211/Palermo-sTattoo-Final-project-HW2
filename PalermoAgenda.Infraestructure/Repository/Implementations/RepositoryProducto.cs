using PalermoAgenda.Infraestructure.Data;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Implementations
{
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly PalermosTattooContext _context;
        public RepositoryProducto(PalermosTattooContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Producto>> ListAsync()
        {
            //Listar los productos incluyendo su categoría, ordenado de forma descendente
            var collection = await _context.Set<Producto>()
                                .Include(x => x.IdCategoriaProductoNavigation)
                                .OrderByDescending(x => x.IdProducto)
                                .AsNoTracking()
                                .ToListAsync();

            return collection;
        }
        public async Task<Producto> FindByIdAsync(int id)
        {
            //Obtener un Producto con su categoría
            var @object = await _context.Set<Producto>()
                                .Where(x => x.IdProducto == id)
                                .Include(x => x.IdCategoriaProductoNavigation)
                                .FirstAsync();
            return @object!;
        }
        public Task<int> AddAsync(Producto entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Producto entity)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Producto>> FindByNameAsync(string nombre)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Producto>> GetProductoByCategoria(int idCategoriaProducto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
