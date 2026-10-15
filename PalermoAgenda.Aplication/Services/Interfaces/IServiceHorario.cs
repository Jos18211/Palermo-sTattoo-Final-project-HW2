
using PalermoAgenda.Application.DTOs;

namespace PalermoAgenda.Application.Services.Interfaces
{
    public interface IServiceHorario
    {
        Task<ICollection<HorarioDTO>>
            ListHorarioEstudioAsync();
    }
}
