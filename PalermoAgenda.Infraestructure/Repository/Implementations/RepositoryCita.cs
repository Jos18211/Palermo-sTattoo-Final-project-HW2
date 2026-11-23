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

    public class RepositoryCita : IRepositoryCita
    {
        private readonly PalermosTattooContext _context;
        private const int EstadoCancelada = 5;
        public RepositoryCita(PalermosTattooContext context)
        {
            _context = context;

        }

        public async Task<ICollection<Cita>> ListByEncargadoAsync(int idEncargado)
        {
            var collection = await _context.Set<Cita>()
                                            .Include(x => x.IdServicioNavigation)
                                            .Include(x => x.IdEstadoCitaNavigation)
                                            .Include(x => x.IdClienteNavigation)
                                            .Include(x => x.IdArtistaNavigation)
                                            //Solo las citas del artista al que pertenece el encargado
                                            .Where(x => x.IdArtistaNavigation.Usuario.Any(u => u.IdUsuario == idEncargado))
                                            .OrderByDescending(x => x.Fecha).ThenByDescending(x => x.HoraInicio)
                                            .AsNoTracking()
                                            .ToListAsync();
            return collection;
        }

        public async Task<Cita> FindByIdAsync(int id, int idEncargado)
        {
            var @object = await _context.Set<Cita>()
              .Include(x => x.IdServicioNavigation)
              .Include(x => x.IdEstadoCitaNavigation)
              .Include(x => x.IdClienteNavigation)
              .Include(x => x.IdArtistaNavigation)
              .Where(x => x.IdCita == id
                       && x.IdArtistaNavigation.Usuario.Any(u => u.IdUsuario == idEncargado))
              .FirstOrDefaultAsync();

            return @object!;
        }
        public async Task<ICollection<Cita>> ListByArtistaAsync(int idArtista)
        {
            //Horarios de una sucursal, en orden cronologico
            return await _context.Set<Cita>()
                .Where(x => x.IdArtista == idArtista)
                .Include(x => x.IdArtistaNavigation)
                .OrderBy(x => x.Fecha).ThenBy(x => x.HoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<ICollection<Cita>> ListByArtistaFechaAsync(int idArtista, DateOnly fecha)
        {
            //Horarios de una sucursal en una fecha (para validar sobreposicion)
            return await _context.Set<Cita>()
                .Where(x => x.IdArtista == idArtista && x.Fecha == fecha && x.IdEstadoCita!= EstadoCancelada)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Cita> FindByIdChangeAsync(int id)
        {
            var @object = await _context.Set<Cita>()
                .Where(x => x.IdCita == id).FirstOrDefaultAsync();

            return @object!;
        }
        public async Task<Cita> FindByIdAsync(int id)
        {
            //Obtener un Producto con su categoría
            var @object = await _context.Set<Cita>()
                                .Where(x => x.IdCita == id)
                                .FirstOrDefaultAsync();
            return @object!;
        }
        public async Task<int> AddAsync(Cita entity)
        {
            // 1. Agregar la Producto
            await _context.Set<Cita>().AddAsync(entity);
            // 2. SaveChanges
            await _context.SaveChangesAsync();

            return entity.IdCita;
        }

        public async Task UpdateAsync(Cita entity)
        {
            await _context.SaveChangesAsync();
        }

        public async Task CancelAsync(int id)
        {
            // Borrado lógico: no se elimina, se desactiva
            var entity = await _context.Set<Cita>().FindAsync(id);
            if (entity == null) return;
            entity.IdEstadoCita = EstadoCancelada;
            await _context.SaveChangesAsync();
        }
    }
}
