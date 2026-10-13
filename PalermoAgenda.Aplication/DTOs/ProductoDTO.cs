using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.DTOs
{
    public record ProductoDTO
    {
        [Display(Name = "Identificador Producto")]
        public int IdProducto { get; set; }

        [Display(Name = "Nombre Producto")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = null!;

        [Display(Name = "Categoría")]
        public int IdCategoriaProducto { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Precio { get; set; }

        public string Marca { get; set; } = null!;

        [Display(Name = "Presentación")]
        public string Presentacion { get; set; } = null!;

        public byte[]? Imagen { get; set; }
        public bool Estado { get; set; }

        public virtual CategoriaDTO IdCategoriaProductoNavigation { get; set; } = null!;
        public virtual List<FacturaDetalleDTO> FacturaDetalle { get; set; } = null!;
    }
}
