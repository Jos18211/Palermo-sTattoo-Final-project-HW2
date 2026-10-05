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
    public class RepositoryAutor : IRepositoryAutor
    {
        private readonly LibreriaContext _context;
        public RepositoryAutor(LibreriaContext context)
        {
            _context = context;
        }
        public Task<Autor> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<ICollection<Autor>> ListAsync()
        {
            //Select * from Autor 
            var collection = await _context.Set<Autor>().ToListAsync(); return collection;
        }
    }
}
