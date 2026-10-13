using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PalermoAgenda.Application.DTOs
{
    public record ServicioDTO
    {

        [Display(Name = "Identificador del Servicio")]
        public int IdServicio { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = null!;
     

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; } = null!;

        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Precio { get; set; }

        [Display(Name = "Duracion (min)")]
        public int DuracionMinutos { get; set; }
       
        public string Estilo { get; set; } = null!;

        public string Complejidad { get; set; } = null!;

        public bool Estado { get; set; }

        public byte[]? Imagen { get; set; }

      
    }
}
