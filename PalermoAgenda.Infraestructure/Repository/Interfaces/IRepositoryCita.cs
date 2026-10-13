using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCita
    {
        Task<ICollection<Cita>> ListByEncargadoAsync(int idEncargado);
        Task<Cita> FindByIdAsync(int id, int idEncargado);
       
    }
}
