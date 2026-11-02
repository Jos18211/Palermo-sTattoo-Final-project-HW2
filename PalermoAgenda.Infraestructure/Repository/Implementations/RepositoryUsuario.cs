
using Microsoft.EntityFrameworkCore;
using PalermoAgenda.Infraestructure.Data;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Implementations
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly PalermosTattooContext _context;

        public RepositoryUsuario(PalermosTattooContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Usuario entity)
        {
            await _context.Set<Usuario>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdUsuario;
        }

        public async Task DeleteAsync(int id)
        {
            var @object = await FindByIdAsync(id);
            _context.Remove(@object);
            _context.SaveChanges();
        }
        public async Task<ICollection<Usuario>> FindByDescriptionAsync(string description)
        {
            var collection = await _context
                                         .Set<Usuario>()
                                         .Where(p => p.Nombre.Contains(description))
                                         .ToListAsync();
            return collection;
        }
        public async Task<Usuario> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Usuario>().FindAsync(id);

            return @object!;
        }
        public async Task<ICollection<Usuario>> FindByNombreRolAsync(string nombre)
        {
            var collection = await _context.Set<Usuario>()
           .Where(u => u.IdRolNavigation.Descripcion == "Cliente" && u.Nombre.Contains(nombre))
                        .AsNoTracking()
                        .ToListAsync();
            return collection;
        }

        public async Task<ICollection<Usuario>> ListAsync()
        {
            var collection = await _context.Set<Usuario>()
                                          .Include(p => p.IdRolNavigation)
                                          .ToListAsync();
            return collection;
        }
        public async Task<Usuario> LoginAsync(string email,string password)
        {
            var @object = await _context.Set<Usuario>()
                                        .Include(b => b.IdRolNavigation)
                                        .Where(p => p.Email == email && p.PasswordHash == password)
                                        .FirstOrDefaultAsync();
            return @object!;
        }
        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}