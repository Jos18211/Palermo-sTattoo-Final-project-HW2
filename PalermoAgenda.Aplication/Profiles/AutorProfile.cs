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
    public class AutorProfile : Profile
    {
        public AutorProfile()
        {
            CreateMap<AutorDTO, Autor>().ReverseMap();
        }
    }
}