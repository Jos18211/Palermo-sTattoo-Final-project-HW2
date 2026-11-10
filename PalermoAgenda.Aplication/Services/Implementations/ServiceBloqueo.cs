using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;

namespace PalermoAgenda.Application.Services.Implementations
{
    public class ServiceBloqueo : IServiceBloqueo
    {
        private readonly IRepositoryBloqueo _repository;
        private readonly IRepositoryHorario _repositoryHorario;
        private readonly IMapper _mapper;

        public ServiceBloqueo(IRepositoryBloqueo repository,
                              IRepositoryHorario repositoryHorario,
                              IMapper mapper)
        {
            _repository = repository;
            _repositoryHorario = repositoryHorario;
            _mapper = mapper;
        }

        public async Task<ICollection<BloqueoDTO>> ListByArtistaAsync(int idArtista)
        {
            var list = await _repository.ListByArtistaAsync(idArtista);
            return _mapper.Map<ICollection<BloqueoDTO>>(list);
        }

        public async Task<BloqueoDTO?> FindByIdAsync(int id)
        {
            var entity = await _repository.FindByIdAsync(id);
            return entity == null ? null : _mapper.Map<BloqueoDTO>(entity);
        }

        public async Task<string?> ValidarAsync(BloqueoDTO dto)
        {
            // 1. Fecha vigente y, si no es todo el día, rango de horas válido
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            if (dto.TodoElDia)
            {
                if (dto.Fecha < hoy)
                    return "La fecha no puede ser anterior a la fecha actual";
            }
            else
            {
                if (dto.HoraInicio == null || dto.HoraFin == null)
                    return "Indique la hora de inicio y de fin, o marque Todo el día";
                var error = ValidacionAgenda.ValidarFechaYRango(dto.Fecha, dto.HoraInicio.Value, dto.HoraFin.Value);
                if (error != null) return error;
            }

            var (inicio, fin) = ValidacionAgenda.RangoBloqueo(dto.TodoElDia, dto.HoraInicio, dto.HoraFin);

            // 2. Sobreposición bloqueo con bloqueo (misma sucursal y fecha)
            var bloqueos = await _repository.ListByArtistaFechaAsync(dto.IdArtista, dto.Fecha);
            foreach (var b in bloqueos)
            {
                var (bInicio, bFin) = ValidacionAgenda.RangoBloqueo(b.TodoElDia, b.HoraInicio, b.HoraFin);
                if (ValidacionAgenda.SeSobreponen(inicio, fin, bInicio, bFin))
                {
                    var detalle = b.TodoElDia ? "un bloqueo de todo el día" : $"un bloqueo de {ValidacionAgenda.Rango(bInicio, bFin)}";
                    return $"Sobreposición de bloqueos: la sucursal ya tiene {detalle} ese día";
                }
            }

            // 3. Sobreposición bloqueo con horario (misma sucursal y fecha)
            var horarios = await _repositoryHorario.ListByArtistaFechaAsync(dto.IdArtista, dto.Fecha);
            var choque = horarios.FirstOrDefault(h =>
                ValidacionAgenda.SeSobreponen(inicio, fin, h.HoraInicio, h.HoraFin));
            if (choque != null)
                return $"Sobreposición de horarios con bloqueos: la sucursal tiene un horario de " +
                       $"{ValidacionAgenda.Rango(choque.HoraInicio, choque.HoraFin)} ese día";

            return null;
        }

        public async Task<int> AddAsync(BloqueoDTO dto)
        {
            var entity = _mapper.Map<Bloqueo>(dto);
            // Regla CK_Bloqueo_Tipo: todo el día va sin horas
            if (entity.TodoElDia)
            {
                entity.HoraInicio = null;
                entity.HoraFin = null;
            }
            return await _repository.AddAsync(entity);
        }
    }
}
