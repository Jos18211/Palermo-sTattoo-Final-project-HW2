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
    public class ServiceCita : IServiceCita
    {
        private readonly IRepositoryCita _repository;
        private readonly IMapper _mapper;
        private readonly IRepositoryBloqueo _repositoryBloqueo;
        private readonly IRepositoryHorario _repositoryHorario;
        private readonly IRepositoryServicio _repositoryServicio;

        // Estado con el que nace toda cita (tabla EstadoCita)
        private const int EstadoPendiente = 1;
        public ServiceCita(IRepositoryCita repository, IRepositoryBloqueo repositoryBloqueo,IRepositoryHorario repositoryHorario,IRepositoryServicio repositoryServicio ,IMapper mapper)
        {
            _repository = repository;
            _repositoryBloqueo = repositoryBloqueo;
            _repositoryHorario = repositoryHorario;
            _repositoryServicio = repositoryServicio;
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
        public async Task<string?> ValidarAsync(CitaDTO dto)
        {



            // 0. El servicio debe existir, estar activo y ser de esta sucursal


            var servicio = await _repositoryServicio.FindByIdAsync(dto.IdServicio);
            if (servicio == null || !servicio.Estado)
                return "El servicio seleccionado no está disponible";

            if (!servicio.IdArtista.Any(a => a.IdArtista == dto.IdArtista))
                return "El servicio seleccionado no lo ofrece este artista";


            dto.HoraFin = dto.HoraInicio.AddMinutes(servicio.DuracionMinutos);

            // 1. Fecha vigente y rango de horas válido
            var error = ValidacionAgenda.ValidarFechaYRango(dto.Fecha, dto.HoraInicio, dto.HoraFin);
            if (error != null) return error;

            // 2. La cita debe caber completa dentro de un horario de la sucursal ese día
            var horarios = await _repositoryHorario.ListByArtistaFechaAsync(dto.IdArtista, dto.Fecha);
            if (!horarios.Any())
                return "El artista no tiene horario de atención en esa fecha";

            bool cabe = horarios.Any(h => h.HoraInicio <= dto.HoraInicio && dto.HoraFin <= h.HoraFin);
            if (!cabe)
                return "La cita debe quedar dentro del horario de atención: " +
                       string.Join(", ", horarios.Select(h => ValidacionAgenda.Rango(h.HoraInicio, h.HoraFin)));


            // 3. Sobreposición con otra cita (misma sucursal y fecha; las canceladas no cuentan)
            var citas = await _repository.ListByArtistaFechaAsync(dto.IdArtista, dto.Fecha);
            var choqueCita = citas.FirstOrDefault(c =>
                ValidacionAgenda.SeSobreponen(dto.HoraInicio, dto.HoraFin, c.HoraInicio, c.HoraFin));
            if (choqueCita != null)
                return $"Sobreposición de citas: el artista ya tiene una cita de " +
                       $"{ValidacionAgenda.Rango(choqueCita.HoraInicio, choqueCita.HoraFin)} ese día";


            // 4. Sobreposición Cita con bloqueo (misma sucursal y fecha)
            var bloqueos = await _repositoryBloqueo.ListByArtistaFechaAsync(dto.IdArtista, dto.Fecha);
            foreach (var b in bloqueos)
            {
                var (inicio, fin) = ValidacionAgenda.RangoBloqueo(b.TodoElDia, b.HoraInicio, b.HoraFin);
                if (ValidacionAgenda.SeSobreponen(dto.HoraInicio, dto.HoraFin, inicio, fin))
                {
                    var detalle = b.TodoElDia ? "un bloqueo de todo el día" : $"un bloqueo de {ValidacionAgenda.Rango(inicio, fin)}";
                    return $"Sobreposición de horario con bloqueo: el artista tiene {detalle} ese día";
                }
            }
            if (dto.TieneAlergias && string.IsNullOrWhiteSpace(dto.DetalleAlergias))
                return "Indique el detalle de las alergias";

            return null;
        }
        public async Task<int> AddAsync(CitaDTO dto)
        {
            var entity = _mapper.Map<Cita>(dto);
            // Datos que pone el sistema, no el formulario
            entity.FechaCreacion = DateTime.Now;
            entity.IdEstadoCita = EstadoPendiente;
            // CK_Cita_Alergias: sin alergias, el detalle debe ir en NULL
            if (!entity.TieneAlergias)
                entity.DetalleAlergias = null;

            // Factura proforma de la cita: cliente, sucursal y el servicio con su cantidad.
            // Al asignarla a entity.Factura, EF la guarda junto con la cita en el mismo SaveChanges
            var servicio = await _repositoryServicio.FindByIdAsync(dto.IdServicio);
            int cantidad = 1;
            decimal subtotal = servicio!.Precio * cantidad;
            decimal impuesto = subtotal * 13 / 100;
            entity.Factura = new Factura
            {
                IdCliente = entity.IdCliente,
                IdArtista = entity.IdArtista,
                FechaFactura = DateTime.Now,
                Estado = "Proforma",
                Subtotal = subtotal,
                PorcentajeImpuesto = 13,
                MontoImpuesto = impuesto,
                Total = subtotal + impuesto
            };
            entity.Factura.FacturaDetalle.Add(new FacturaDetalle
            {
                IdServicio = servicio.IdServicio,
                Cantidad = cantidad,
                PrecioUnitario = servicio.Precio,
                Subtotal = subtotal
            });

            return await _repository.AddAsync(entity);
        }
        public async Task CancelAsync(int id)
        {
            await _repository.CancelAsync(id);
        }
        public async Task UpdateAsync(int id, CitaDTO dto)
        {
            //Obtenga el modelo original a actualizar
            var @object = await _repository.FindByIdAsync(id);
            //       source, destination
            var entity = _mapper.Map(dto, @object!);


            await _repository.UpdateAsync(entity);
        }
    }
}
