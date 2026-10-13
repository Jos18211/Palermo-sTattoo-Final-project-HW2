using System;
using System.Collections.Generic;

namespace PalermoAgenda.Infraestructure.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int IdCategoriaProducto { get; set; }

    public decimal Precio { get; set; }

    public string Marca { get; set; } = null!;

    public string Presentacion { get; set; } = null!;

    public byte[]? Imagen { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<FacturaDetalle> FacturaDetalle { get; set; } = new List<FacturaDetalle>();

    public virtual CategoriaProducto IdCategoriaProductoNavigation { get; set; } = null!;
}
