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
    }
}
