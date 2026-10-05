using PalermoAgenda.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalermoAgenda.Application.DTOs
{
    public record AutorDTO
    { 
        public int IdAutor { get; set; }
        public string Nombre { get; set; } = null!;
        public virtual List<Libro> Libro { get; set; } = new List<Libro>();
    }
}