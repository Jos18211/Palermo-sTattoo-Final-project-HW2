using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Infraestructure.Models;

namespace PalermoAgenda.Application.Profiles
{
    public class HorarioProfile : Profile
    {
        public HorarioProfile()
        {
            CreateMap<HorarioDTO, Horario>().ReverseMap();
        }
    }
}
