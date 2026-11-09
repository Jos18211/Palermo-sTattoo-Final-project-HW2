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
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(500)]
        public string Descripcion { get; set; } = null!;

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1, int.MaxValue,ErrorMessage ="Selecione una categoria")]
        public int IdCategoriaProducto { get; set; }

        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Required(ErrorMessage = "{0} es requerido")]
        [Range(1,99999999, ErrorMessage = "{0} debe ser mayor a cero")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50)]
        public string Marca { get; set; } = null!;

        [Display(Name = "Presentación")]
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(50)]
        public string Presentacion { get; set; } = null!;

        public byte[]? Imagen { get; set; }
        public bool Estado { get; set; }

        [Display(Name = "Existencias")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} no puede ser negativo")]
        public int Stock { get; set; }

        public virtual CategoriaDTO? IdCategoriaProductoNavigation { get; set; } = null!;
        public virtual List<FacturaDetalleDTO>? FacturaDetalle { get; set; } = null!;
    }
}
