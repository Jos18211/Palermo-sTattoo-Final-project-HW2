using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.DTOs
{
    public record UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Identificacion { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public DateOnly FechaNacimiento { get; set; }
        public string PasswordHash { get; set; } = null!;
        public int IdRol { get; set; }
        public int? IdArtista { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public virtual RolDTO IdRolNavigation { get; set; } = null!;
        public virtual ArtistaDTO? IdArtistaNavigation { get; set; }
        public virtual List<FacturaDTO> Factura { get; set; } = null!;
    }
}
