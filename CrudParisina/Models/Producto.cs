using System;
using System.Collections.Generic;

namespace CrudParisina.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int? IdCategoria { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string DescripcionProducto { get; set; } = null!;

    public double PrecioProducto { get; set; }

    public byte[]? ImagenProducto { get; set; }

    public bool? EstadoProducto { get; set; }

    public virtual Categoria? IdCategoriaNavigation { get; set; }
}
