using Microsoft.EntityFrameworkCore;
using PalermoAgenda.Infraestructure.Data;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
                                .Include(x => x.Usuario)
                                .FirstOrDefaultAsync();

            return @object!;
        }
        public async Task<ICollection<Artista>> ListAsync()
        {
            //Select * from Artista
            var collection = await _context.Set<Artista>()
                .Where(x => x.Estado == true)
                .ToListAsync();
                                         
            return collection;
        }
        public async Task<ICollection<Artista>> ListAsyncAdmin()
        {
            //Select * from Artista
            var collection = await _context.Set<Artista>()
              .Include(x => x.Usuario)
              .AsNoTracking()
              .ToListAsync();

            return collection;
        }
        public async Task<int> AddAsync(Artista entity, int[] encargados)
        {
            // Encargados elegidos que siguen libres (validación del lado del servidor)
            var usuarios = await _context.Set<Usuario>()
                .Where(u => encargados.Contains(u.IdUsuario) && u.IdRol == 2 && u.IdArtista == null)
                .ToListAsync();
            foreach (var u in usuarios) entity.Usuario.Add(u);   // EF asigna el IdArtista

            await _context.Set<Artista>().AddAsync(entity);
            await _context.SaveChangesAsync();                    // un solo guardado
            return entity.IdArtista;
        }

        public async Task DeleteAsync(int id)
        {
            // Borrado lógico: no se elimina, se desactiva
            var entity = await _context.Set<Artista>().FindAsync(id);
            if (entity == null) return;
            entity.Estado = false;
            await _context.SaveChangesAsync();
        }
      

        public async Task UpdateAsync(Artista entity, int[] encargados)
        {
            // 1. Liberar los que se deseleccionaron
            foreach (var u in entity.Usuario.Where(u => !encargados.Contains(u.IdUsuario)).ToList())
            {
                u.IdArtista = null;
                entity.Usuario.Remove(u);
            }

            // 2. Asignar los nuevos (solo si siguen libres)
            var nuevos = await _context.Set<Usuario>()
        .Where(u => encargados.Contains(u.IdUsuario) && u.IdRol == 2 && u.IdArtista == null)
        .ToListAsync();
            foreach (var u in nuevos) entity.Usuario.Add(u);

            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExisteEmailAsync(string email, int idExcluir)
        {
            // El email es único; al editar se excluye la propia sucursal
            return await _context.Set<Artista>()
                .AnyAsync(x => x.Email == email.Trim() && x.IdArtista != idExcluir);
        }

        public async Task<ICollection<Artista>> FindByNameAsync(string nombre)
        {
            var collection = await _context.Set<Artista>()
                               .Where(x => x.Nombre.Contains(nombre))
                               .OrderByDescending(x => x.IdArtista)
                               .AsNoTracking()
                               .ToListAsync();

            return collection;

        }


    }
}
