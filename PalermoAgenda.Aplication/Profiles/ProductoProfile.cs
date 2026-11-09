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
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<Producto, ProductoDTO>();
            CreateMap<ProductoDTO, Producto>()
                .ForMember(d => d.IdCategoriaProductoNavigation, o => o.Ignore());
        }
    }
}
