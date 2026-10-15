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
            //Horarios de todos los artistas por dia
            var collection = await _context.Set<Horario>()
                .OrderBy(x => x.DiaSemana)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
