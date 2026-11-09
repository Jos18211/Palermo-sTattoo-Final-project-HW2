using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Infraestructure.Models;


namespace PalermoAgenda.Application.Profiles
{
    public class ServicioProfile : Profile
    {
        public ServicioProfile()
        {
            CreateMap<Servicio, ServicioDTO>();
            CreateMap<ServicioDTO, Servicio>()
                .ForMember(d => d.IdArtista, o => o.Ignore())
                .ForMember(d => d.Cita, o => o.Ignore())
                .ForMember(d => d.FacturaDetalle, o => o.Ignore());
        }
    }
}
