using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Infraestructure.Models;

namespace PalermoAgenda.Application.Profiles
{
    public class BloqueoProfile : Profile
    {
        public BloqueoProfile()
        {
            CreateMap<Bloqueo, BloqueoDTO>()
                .ForMember(d => d.NombreSucursal,
                           o => o.MapFrom(s => s.IdArtistaNavigation != null ? s.IdArtistaNavigation.Nombre : null));
            CreateMap<BloqueoDTO, Bloqueo>()
                .ForMember(d => d.IdArtistaNavigation, o => o.Ignore());
        }
    }
}
