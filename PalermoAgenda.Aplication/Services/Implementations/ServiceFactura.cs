using AutoMapper;
using PalermoAgenda.Application.DTOs;
using PalermoAgenda.Application.Services.Interfaces;
using PalermoAgenda.Infraestructure.Models;
using PalermoAgenda.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PalermoAgenda.Application.Services.Implementations
{
    public class ServiceFactura : IServiceFactura
    {
        private readonly IRepositoryFactura _repositoryFactura;
        private readonly IMapper _mapper;
        public ServiceFactura(IRepositoryFactura repositoryFactura, IMapper mapper)
        {
            _repositoryFactura = repositoryFactura;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(FacturaDTO dto)
        {
            var @object = _mapper.Map<Factura>(dto);
            return await _repositoryFactura.AddAsync(@object);
        }

        public async Task<FacturaDTO> FindByIdAsync(int id)
        {
            var @object = await _repositoryFactura.FindByIdAsync(id);
            var objectMapped = _mapper.Map<FacturaDTO>(@object);
            return objectMapped;
        }

        public async Task<FacturaDTO> FindByIdChangeAsync(int id)
        {
            var @object = await _repositoryFactura.FindByIdChangeAsync(id);
            var objectMapped = _mapper.Map<FacturaDTO>(@object);
            return objectMapped;
        }

        public async Task<int> GetNextNumberFactura()
        {
            int nextReceipt = await _repositoryFactura.GetNextNumberFactura();
            return nextReceipt + 1;
        }

        public async Task<ICollection<FacturaDTO>> ListAsync()
        {
            var list = await _repositoryFactura.ListAsync();
            var collection = _mapper.Map<ICollection<FacturaDTO>>(list);
            return collection;
        }

    }
}
