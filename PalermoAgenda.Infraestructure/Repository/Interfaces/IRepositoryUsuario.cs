using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Infraestructure.Repository.Interfaces;
public interface IRepositoryUsuario
{
    Task<ICollection<Usuario>> FindByDescriptionAsync(string description);
    Task<ICollection<Usuario>> ListAsync();
    Task<Usuario> FindByIdAsync(int id);
    Task<ICollection<Usuario>> FindByNombreRolAsync(string nombre);
    Task<Usuario> LoginAsync(string email,string password );
    Task<int> AddAsync(Usuario entity);
    Task DeleteAsync(int id);
    Task UpdateAsync();
}