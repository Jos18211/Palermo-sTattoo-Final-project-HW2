using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Infraestructure.Models;

namespace PalermoAgenda.Application.Profiles
{
    public class HorarioProfile : Profile
    {
        public HorarioProfile()
        {
            CreateMap<Horario, HorarioDTO>()
                .ForMember(d => d.NombreSucursal,
                           o => o.MapFrom(s => s.IdArtistaNavigation != null ? s.IdArtistaNavigation.Nombre : null));
            CreateMap<HorarioDTO, Horario>()
                .ForMember(d => d.IdArtistaNavigation, o => o.Ignore());
        }
    }
}
