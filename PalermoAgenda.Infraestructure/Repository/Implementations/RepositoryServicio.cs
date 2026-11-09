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
                .FirstOrDefaultAsync();
            return @object;
        }

        public async Task<int> AddAsync(Servicio entity)
        {
            // 1. Agregar la Producto 
            await _context.Set<Servicio>().AddAsync(entity);
            // 2. SaveChanges
            await _context.SaveChangesAsync();

            return entity.IdServicio;
        }

        public async Task UpdateAsync(Servicio entity)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Servicio>> FindByNameAsync(string nombre)
        {
            var collection = await _context.Set<Servicio>()
                               .Where(x => x.Nombre.Contains(nombre))
                               .OrderByDescending(x => x.IdServicio)
                               .AsNoTracking()
                               .ToListAsync();

            return collection;

        }

        public async Task<ICollection<Servicio>> GetServicioByEstilo(string estilo)
        {
            var collection = await _context.Set<Servicio>()
                               .Where(x => x.Estilo == estilo)
                               .OrderByDescending(x => x.IdServicio)
                               .AsNoTracking()
                               .ToListAsync();

            return collection;



        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}

