using System;
using System.Collections.Generic;

namespace LECCIONCLIENTES.Models;

public partial class Cliente
{
    public int Idc { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public DateTime? FechaCreacion { get; set; }
}
