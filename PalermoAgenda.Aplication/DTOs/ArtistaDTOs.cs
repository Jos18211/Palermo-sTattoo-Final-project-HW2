using AutoMapper;
using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.DTOs
{
    public record ArtistaDTO
    {
        public int IdArtista { get; set; }

        [DisplayName("Nombre del Artista")]
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(100, ErrorMessage = "El nombre del artista no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres.")]
        public string Descripcion { get; set; } = null!;

        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "{0} es requerido")]
        [RegularExpression(@"^\d{4}-?\d{4}$", ErrorMessage = "Formato 8888-8888")]
        public string Telefono { get; set; } = null!;

        [DisplayName("Dirección")]
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(250, ErrorMessage = "La dirección no puede tener más de 250 caracteres.")]
        public string Direccion { get; set; } = null!;

        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
        [StringLength(100, ErrorMessage = "El Email no puede tener más de 100 caracteres.")]
        public string Email { get; set; } = null!;
        public byte[]? Imagen { get; set; }
        public bool Estado { get; set; }

        [Display(Name = "Encargados")]
        // lo que llega del <select multiple>
        public int[] SelectedEncargados { get; set; } = Array.Empty<int>();

        // para Detalle y preselección
        public ICollection<UsuarioDTO>? Usuario { get; set; }
  

    }
}
