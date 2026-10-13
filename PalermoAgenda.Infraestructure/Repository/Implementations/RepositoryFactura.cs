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
    public class RepositoryFactura : IRepositoryFactura
    {
        private readonly PalermosTattooContext _context;
        public RepositoryFactura(PalermosTattooContext context)
        {
            _context = context;
        }
        public Task<int> AddAsync(Factura entity)
        {
            throw new NotImplementedException();

        }

        public async Task<Factura> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Factura>()
               .Include(x => x.FacturaDetalle).ThenInclude(d => d.IdProductoNavigation)
               .Include(x => x.FacturaDetalle).ThenInclude(d => d.IdServicioNavigation)
               .Include(x => x.IdClienteNavigation)
               .Include(x => x.IdArtistaNavigation)
               .Where(x => x.IdFactura == id).FirstOrDefaultAsync();

            return @object!;
        }
        public async Task<ICollection<Factura>> ListAsync()
        {
            var collection = await _context.Set<Factura>()
                                            .Include(x => x.IdClienteNavigation)
                                            .OrderByDescending(x => x.FechaFactura)
                                            .ThenByDescending(x => x.IdFactura)
                                            .AsNoTracking()
                                            .ToListAsync();
            return collection;
        }
        public async Task<int> GetNextNumberFactura()
        {
            int current = 0;

            string sql = string.Format("SELECT IDENT_CURRENT ('Factura') AS Current_Identity;");

            System.Data.DataTable dataTable = new System.Data.DataTable();

            System.Data.Common.DbConnection connection = _context.Database.GetDbConnection();
            System.Data.Common.DbProviderFactory dbFactory = System.Data.Common.DbProviderFactories.GetFactory(connection!)!;
            using (var cmd = dbFactory!.CreateCommand())
            {
                cmd!.Connection = connection;
                cmd.CommandText = sql;
                using (System.Data.Common.DbDataAdapter adapter = dbFactory.CreateDataAdapter()!)
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }

            current = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            return await Task.FromResult(current);
        }

        public async Task<Factura> FindByIdChangeAsync(int id)
        {
            var @object = await _context.Set<Factura>()
                .Where(x => x.IdFactura == id).FirstOrDefaultAsync();

            return @object!;
        }
        public async Task<ICollection<Factura>> FacturaByClientId(int id)
        {
            var response = await _context.Set<Factura>()
                           .Include(p => p.FacturaDetalle)
                           .Where(p => p.IdCliente == id).ToListAsync();

            return response;

        }

    }
}