
using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryFactura
    {
        Task<ICollection<Factura>> ListAsync();
        Task<Factura> FindByIdAsync(int id);
        Task<Factura> FindByIdChangeAsync(int id);
        Task<int> AddAsync(Factura entity);
        Task<int> GetNextNumberFactura();
        Task<ICollection<Factura>> FacturaByClientId(int id);
    }
}
