using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Infraestructure.Models;

namespace PalermoAgenda.Application.Profiles
{
    public class EstadoCitaProfile: Profile
    {
        public EstadoCitaProfile()
        {
            CreateMap<EstadoCitaDTO, EstadoCita>().ReverseMap();
        }
    }
}
