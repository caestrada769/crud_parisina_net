using System;
using System.Collections.Generic;

namespace CrudParisina.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public int? IdUsuario { get; set; }

    public string NombreCliente { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public bool? EstadoClientes { get; set; }
}
