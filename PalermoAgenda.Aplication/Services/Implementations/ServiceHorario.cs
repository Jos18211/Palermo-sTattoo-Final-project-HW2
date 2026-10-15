using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using PalermoAgenda.Infraestructure.Repository.Interfaces;


namespace PalermoAgenda.Application.Services.Implementations
{
    public class ServiceHorario : IServiceHorario
    {
        private readonly IRepositoryHorario _repository;
        private readonly IMapper _mapper;

        public ServiceHorario(
            IRepositoryHorario repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<HorarioDTO>>
            ListHorarioEstudioAsync()
        {
            var list = await _repository.ListAsync();
            var horarios = _mapper
                .Map<ICollection<HorarioDTO>>(list);

            //Un registro por dia: abre con el primer
            //artista y cierra con el ultimo
            var estudio = horarios
                .GroupBy(h => h.DiaSemana)
                .OrderBy(g => g.Key)
                .Select(g => new HorarioDTO
                {
                    DiaSemana = g.Key,
                    HoraInicio = g.Min(h => h.HoraInicio),
                    HoraFin = g.Max(h => h.HoraFin)
                })
                .ToList();
            return estudio;
        }
    }
} 
