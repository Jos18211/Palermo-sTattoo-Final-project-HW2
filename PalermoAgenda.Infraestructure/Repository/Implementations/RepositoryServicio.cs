using Microsoft.EntityFrameworkCore;
using PalermoAgenda.Infraestructure.Data;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;


namespace PalermoAgenda.Infraestructure.Repository.Implementations
{
    public class RepositoryServicio : IRepositoryServicio
    {
        private readonly PalermosTattooContext _context;
        public RepositoryServicio( PalermosTattooContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Servicio>> ListAsync()
        {
            //Servicios activos ordenados por precio
            var collection = await _context.Set<Servicio>()
                .Where(x => x.Estado)
                .OrderBy(x => x.Precio)
                .AsNoTracking()
                .ToListAsync();

            return collection;
        }
        public async Task<Servicio?> FindByIdAsync(int id)
        {
            //Servicio con los artistas que lo realizan 
            var @object = await _context.Set<Servicio>()
                .Where(x => x.IdServicio == id)
                .Include(x => x.IdArtista)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return @object;
        }
    }
}
