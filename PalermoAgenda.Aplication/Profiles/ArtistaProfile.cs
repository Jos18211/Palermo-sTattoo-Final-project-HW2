using AutoMapper;

using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.Profiles
{
    public class ArtistaProfile : Profile
    {
        public ArtistaProfile()
        {
            CreateMap<Artista, ArtistaDTO>()
                .ForMember(d => d.SelectedEncargados, o => o.Ignore());
            CreateMap<ArtistaDTO, Artista>()
                .ForMember(d => d.Usuario, o => o.Ignore())
                .ForMember(d => d.Cita, o => o.Ignore())
                .ForMember(d => d.Factura, o => o.Ignore())
                .ForMember(d => d.Horario, o => o.Ignore())
                .ForMember(d => d.Bloqueo, o => o.Ignore())
                .ForMember(d => d.IdServicio, o => o.Ignore());
        }
    }
}
