using PalermosTattooAgenda0.Web.Models;

namespace PalermosTattooAgenda0.Web.Services
{
    public interface IServiceInstagram
    {
        Task<List<PublicacionInstagram>> ListAsync(
            int cantidad);
    }
}