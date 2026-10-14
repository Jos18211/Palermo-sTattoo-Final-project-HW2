using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
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
        public async Task<ServicioDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<ServicioDTO>(@object);
        }
    }
}

