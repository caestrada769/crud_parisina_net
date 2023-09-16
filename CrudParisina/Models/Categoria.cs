using System;
using System.Collections.Generic;

namespace CrudParisina.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public string DescripcionCategoria { get; set; } = null!;

    public byte[]? ImagenCategoria { get; set; }

    public bool? EstadoCategoria { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
