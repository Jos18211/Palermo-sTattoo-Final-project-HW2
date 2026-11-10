using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;

namespace PalermoAgenda.Application.Services.Implementations
{
    public class ServiceHorario : IServiceHorario
    {
        private readonly IRepositoryHorario _repository;
        private readonly IRepositoryBloqueo _repositoryBloqueo;
        private readonly IMapper _mapper;

        public ServiceHorario(
            IRepositoryHorario repository,
            IRepositoryBloqueo repositoryBloqueo,
            IMapper mapper)
        {
            _repository = repository;
            _repositoryBloqueo = repositoryBloqueo;
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

        public async Task<ICollection<HorarioDTO>> ListByArtistaAsync(int idArtista)
        {
            var list = await _repository.ListByArtistaAsync(idArtista);
            return _mapper.Map<ICollection<HorarioDTO>>(list);
        }

        public async Task<HorarioDTO?> FindByIdAsync(int id)
        {
            var entity = await _repository.FindByIdAsync(id);
            return entity == null ? null : _mapper.Map<HorarioDTO>(entity);
        }

        public async Task<string?> ValidarAsync(HorarioDTO dto)
        {
            // 1. Fecha vigente y rango de horas válido
            var error = ValidacionAgenda.ValidarFechaYRango(dto.Fecha, dto.HoraInicio, dto.HoraFin);
            if (error != null) return error;

            // 2. Sobreposición horario con horario (misma sucursal y fecha)
            var horarios = await _repository.ListByArtistaFechaAsync(dto.IdArtista, dto.Fecha);
            var choqueHorario = horarios.FirstOrDefault(h =>
                ValidacionAgenda.SeSobreponen(dto.HoraInicio, dto.HoraFin, h.HoraInicio, h.HoraFin));
            if (choqueHorario != null)
                return $"Sobreposición de horarios: la sucursal ya tiene un horario de " +
                       $"{ValidacionAgenda.Rango(choqueHorario.HoraInicio, choqueHorario.HoraFin)} ese día";

            // 3. Sobreposición horario con bloqueo (misma sucursal y fecha)
            var bloqueos = await _repositoryBloqueo.ListByArtistaFechaAsync(dto.IdArtista, dto.Fecha);
            foreach (var b in bloqueos)
            {
                var (inicio, fin) = ValidacionAgenda.RangoBloqueo(b.TodoElDia, b.HoraInicio, b.HoraFin);
                if (ValidacionAgenda.SeSobreponen(dto.HoraInicio, dto.HoraFin, inicio, fin))
                {
                    var detalle = b.TodoElDia ? "un bloqueo de todo el día" : $"un bloqueo de {ValidacionAgenda.Rango(inicio, fin)}";
                    return $"Sobreposición de horario con bloqueo: la sucursal tiene {detalle} ese día";
                }
            }
            return null;
        }

        public async Task<int> AddAsync(HorarioDTO dto)
        {
            var entity = _mapper.Map<Horario>(dto);
            // El día de la semana se deriva de la fecha
            entity.DiaSemana = ValidacionAgenda.DiaSemana(dto.Fecha);
            return await _repository.AddAsync(entity);
        }
    }
}
