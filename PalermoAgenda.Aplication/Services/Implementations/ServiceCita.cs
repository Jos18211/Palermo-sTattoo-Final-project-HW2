using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using PalermoAgenda.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.Services.Implementations
{
    public class ServiceCita : IServiceCita
    {
        private readonly IRepositoryCita _repository;
        private readonly IMapper _mapper;
        public ServiceCita(IRepositoryCita repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CitaDTO> FindByIdAsync(int id, int idEncargado)
        {
            var @object = await _repository.FindByIdAsync(id, idEncargado);
            var objectMapped = _mapper.Map<CitaDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<CitaDTO>> ListByEncargadoAsync(int idEncargado)
        {
            var list = await _repository.ListByEncargadoAsync(idEncargado);
            var collection = _mapper.Map<ICollection<CitaDTO>>(list);
            return collection;
        }
    }
}
