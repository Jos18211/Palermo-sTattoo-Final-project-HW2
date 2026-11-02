using PalermoAgenda.Infraestructure.Data;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PalermoAgenda.Infraestructure.Repository.Implementations
{
    public class RepositoryCategoria : IRepositoryCategoria
    {
        private readonly PalermosTattooContext _context;
        public RepositoryCategoria(PalermosTattooContext context)
        {
            _context = context;
        }
        public async Task<CategoriaProducto> FindByIdAsync(int id)
        {
            //Obtener un Artista
            var @object = await _context.Set<CategoriaProducto>()
                                .Where(x => x.IdCategoriaProducto == id)
                                .FirstOrDefaultAsync();
            return @object!;
        }
        public async Task<ICollection<CategoriaProducto>> ListAsync()
        {
            //Select * from Artista
            var collection = await _context.Set<CategoriaProducto>().ToListAsync(); return collection;
        }
    }
}
