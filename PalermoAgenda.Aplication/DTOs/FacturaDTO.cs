using PalermoAgenda.Infraestructure.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.DTOs
{
    public record FacturaDTO
    {
        [Display(Name = "Factura #")]
        public int IdFactura { get; set; }

        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Display(Name = "Artista")]
        public int IdArtista { get; set; }

        [Display(Name = "Cita")]
        public int? IdCita { get; set; }

        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FechaFactura { get; set; }

        public string Estado { get; set; } = null!;

        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Subtotal { get; set; }

        [Display(Name = "% Impuesto")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal PorcentajeImpuesto { get; set; }

        [Display(Name = "Impuesto")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal MontoImpuesto { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Total { get; set; }

        [Display(Name = "Cliente")]
        public virtual UsuarioDTO IdClienteNavigation { get; set; } = null!;

        [Display(Name = "Artista")]
        public virtual ArtistaDTO IdArtistaNavigation { get; set; } = null!;
        public virtual List<FacturaDetalleDTO> FacturaDetalle { get; set; } = null!;
    }
}
