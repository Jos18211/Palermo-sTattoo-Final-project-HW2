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
    public class CitaProfile : Profile 
    {
        public CitaProfile()
        {
            CreateMap<CitaDTO, Cita>().ReverseMap();
        }   
    }
}
