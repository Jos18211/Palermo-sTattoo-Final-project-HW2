
using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryServicio
    {
       
        Task<ICollection<Servicio>> ListAsync();
        Task<Servicio?> FindByIdAsync(int id);

    }
}


