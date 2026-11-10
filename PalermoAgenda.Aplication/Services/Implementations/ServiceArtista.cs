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
    public class ServiceArtista : IServiceArtista
    {
        private readonly IRepositoryArtista _repository; private readonly IMapper _mapper;
        public ServiceArtista(IRepositoryArtista repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ArtistaDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ArtistaDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<ArtistaDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();
            //Map List<Artista> a ICollection<ArtistaDTO>
            var collection = _mapper.Map<ICollection<ArtistaDTO>>(list);
            // Return lista
            return collection;
        }

        public async Task<ICollection<ArtistaDTO>> ListAsyncAdmin()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsyncAdmin();
            //Map List<Artista> a ICollection<ArtistaDTO>
            var collection = _mapper.Map<ICollection<ArtistaDTO>>(list);
            // Return lista
            return collection;
        }
        public async Task<bool> ExisteEmailAsync(string email, int idExcluir)
        {
            return await _repository.ExisteEmailAsync(email, idExcluir);
        }

        public async Task<ICollection<ArtistaDTO>> FindByNameAsync(string nombre)
        {
            var list = await _repository.FindByNameAsync(nombre);

            var collection = _mapper.Map<ICollection<ArtistaDTO>>(list);

            return collection;

        }

        public async Task<int> AddAsync(ArtistaDTO dto)
        {
            // Map ProductoDTO to Producto
            var objectMapped = _mapper.Map<Artista>(dto);

            // Return
            return await _repository.AddAsync(objectMapped, dto.SelectedEncargados);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task UpdateAsync(int id, ArtistaDTO dto)
        {
            //Obtenga el modelo original a actualizar
            var @object = await _repository.FindByIdAsync(id);
            //       source, destination
            var entity = _mapper.Map(dto, @object!);

            await _repository.UpdateAsync(entity, dto.SelectedEncargados);             // en UpdateAsync

            
        }

    }
}
