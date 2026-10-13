using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.DTOs
{
    public record FacturaDetalleDTO
    {
        public int IdFacturaDetalle { get; set; }
        public int IdFactura { get; set; }

        [Display(Name = "Producto")]
        public int? IdProducto { get; set; }

        [Display(Name = "Servicio")]
        public int? IdServicio { get; set; }

        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "Precio")]
        public decimal PrecioUnitario { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        public virtual ProductoDTO? IdProductoNavigation { get; set; }
        public virtual ServicioDTO? IdServicioNavigation { get; set; }

        public virtual FacturaDTO IdFacturaNavigation { get; set; } = null!;
    }
}
