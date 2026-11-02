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
    public class RepositoryArtista : IRepositoryArtista
    {
        private readonly PalermosTattooContext _context;
        public RepositoryArtista(PalermosTattooContext context)
        {
            _context = context;
        }
        public async Task<Artista> FindByIdAsync(int id)
        {
            //Obtener un Artista
            var @object = await _context.Set<Artista>()
                                .Where(x => x.IdArtista == id)
                                .FirstOrDefaultAsync();
            return @object!;
        }
        public async Task<ICollection<Artista>> ListAsync()
        {
            //Select * from Artista
            var collection = await _context.Set<Artista>().ToListAsync(); return collection;
        }
    }
}
