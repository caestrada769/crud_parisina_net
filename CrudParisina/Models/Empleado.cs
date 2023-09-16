using System;
using System.Collections.Generic;

namespace CrudParisina.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdArea { get; set; }

    public string NombreEmpleado { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public bool? EstadoEmpleado { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
