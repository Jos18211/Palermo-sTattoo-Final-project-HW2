using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermosTattooAgenda0.Web.Models
{
    public record PublicacionInstagram
    {
        public string Imagen { get; init; } = "";
        public string Enlace { get; init; } = "";
        public string? Video { get; init; }
    }
}