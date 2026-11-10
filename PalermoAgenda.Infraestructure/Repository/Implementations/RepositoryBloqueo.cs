using Microsoft.EntityFrameworkCore;
using PalermoAgenda.Infraestructure.Data;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;

namespace PalermoAgenda.Infraestructure.Repository.Implementations
{
    public class RepositoryBloqueo : IRepositoryBloqueo
    {
        private readonly PalermosTattooContext _context;

        public RepositoryBloqueo(PalermosTattooContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Bloqueo>> ListByArtistaAsync(int idArtista)
        {
            //Bloqueos de una sucursal, en orden cronologico
            return await _context.Set<Bloqueo>()
                .Where(x => x.IdArtista == idArtista)
                .Include(x => x.IdArtistaNavigation)
                .OrderBy(x => x.Fecha).ThenBy(x => x.HoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<Bloqueo>> ListByArtistaFechaAsync(int idArtista, DateOnly fecha)
        {
            //Bloqueos de una sucursal en una fecha (para validar sobreposicion)
            return await _context.Set<Bloqueo>()
                .Where(x => x.IdArtista == idArtista && x.Fecha == fecha)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Bloqueo?> FindByIdAsync(int id)
        {
            return await _context.Set<Bloqueo>()
                .Where(x => x.IdBloqueo == id)
                .Include(x => x.IdArtistaNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddAsync(Bloqueo entity)
        {
            await _context.Set<Bloqueo>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdBloqueo;
        }
    }
}
