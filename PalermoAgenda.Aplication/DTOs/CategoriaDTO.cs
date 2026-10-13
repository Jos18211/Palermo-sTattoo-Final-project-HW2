using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.DTOs
{
    public record CategoriaDTO
    {
        public int IdCategoriaProducto { get; set; }
        public string Nombre { get; set; } = null!;
        public virtual List<ProductoDTO> Producto { get; set; } = null!;
    }
}
