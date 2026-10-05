using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Implementations;
using PalermoAgenda.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.Services.Implementations
{
    public class ServiceAutor : IServiceAutor
    {
        private readonly IRepositoryAutor _repository; private readonly IMapper _mapper;
        public ServiceAutor(IRepositoryAutor repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<AutorDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<AutorDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<AutorDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();
            //Map List<Autor> a ICollection<BodegaDTO> 
            var collection = _mapper.Map<ICollection<AutorDTO>>(list);
            // Return lista
            return collection;
        }
    }
}