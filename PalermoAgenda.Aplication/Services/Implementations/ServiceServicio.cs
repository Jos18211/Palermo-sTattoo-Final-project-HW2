using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;


namespace PalermoAgenda.Application.Services.Implementations
{
    public class ServiceServicio : IServiceServicio
    {
        private readonly IRepositoryServicio _repository; private readonly IMapper _mapper;
        public ServiceServicio(IRepositoryServicio repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<ServicioDTO>> ListAsync()
        {
            //Obtener datos del repositorio
            var list = await _repository.ListAsync();
            //Map List<Artista> a ICollection<ArtistaDTO>
            var collection = _mapper.Map<ICollection<ServicioDTO>>(list);
            // Return lista
            return collection;
        }
        public async Task<ICollection<ServicioDTO>> FindByNameAsync(string nombre)
        {
            var list = await _repository.FindByNameAsync(nombre);

            var collection = _mapper.Map<ICollection<ServicioDTO>>(list);

            return collection;

        }
        public async Task<ServicioDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<ServicioDTO>(@object);
        }

        public async Task<int> AddAsync(ServicioDTO dto)
        {
            // Map ProductoDTO to Producto
            var objectMapped = _mapper.Map<Servicio>(dto);

            // Return
            return await _repository.AddAsync(objectMapped);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task UpdateAsync(int id, ServicioDTO dto)
        {
            //Obtenga el modelo original a actualizar
            var @object = await _repository.FindByIdAsync(id);
            //       source, destination
            var entity = _mapper.Map(dto, @object!);


            await _repository.UpdateAsync(entity);
        }


        public async Task<ICollection<ServicioDTO>> GetServicioByEstilo(string estilo)
        {
            // Get data from Repository
            var list = await _repository.GetServicioByEstilo(estilo);
            // Map List<Producto> to ICollection<ProductoDTO>
            var collection = _mapper.Map<ICollection<ServicioDTO>>(list);
            // Return Data
            return collection;
        }
    }
}

