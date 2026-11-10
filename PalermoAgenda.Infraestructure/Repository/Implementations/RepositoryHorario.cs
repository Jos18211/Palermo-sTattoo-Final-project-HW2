using Microsoft.EntityFrameworkCore;
using PalermoAgenda.Infraestructure.Data;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;

namespace PalermoAgenda.Infraestructure.Repository.Implementations
{
    public class RepositoryHorario : IRepositoryHorario
    {
        private readonly PalermosTattooContext _context;

        public RepositoryHorario(
            PalermosTattooContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Horario>> ListAsync()
        {
            //Horarios de todos los artistas por dia (lo usa el Home)
            var collection = await _context.Set<Horario>()
                .OrderBy(x => x.DiaSemana)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }

        public async Task<ICollection<Horario>> ListByArtistaAsync(int idArtista)
        {
            //Horarios de una sucursal, en orden cronologico
            return await _context.Set<Horario>()
                .Where(x => x.IdArtista == idArtista)
                .Include(x => x.IdArtistaNavigation)
                .OrderBy(x => x.Fecha).ThenBy(x => x.HoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<Horario>> ListByArtistaFechaAsync(int idArtista, DateOnly fecha)
        {
            //Horarios de una sucursal en una fecha (para validar sobreposicion)
            return await _context.Set<Horario>()
                .Where(x => x.IdArtista == idArtista && x.Fecha == fecha)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Horario?> FindByIdAsync(int id)
        {
            return await _context.Set<Horario>()
                .Where(x => x.IdHorario == id)
                .Include(x => x.IdArtistaNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddAsync(Horario entity)
        {
            await _context.Set<Horario>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdHorario;
        }
    }
}
