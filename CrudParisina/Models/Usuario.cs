using System;
using System.Collections.Generic;

namespace CrudParisina.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int? IdRol { get; set; }

    public string Correo { get; set; } = null!;

    public bool? EstadoUsuario { get; set; }

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
