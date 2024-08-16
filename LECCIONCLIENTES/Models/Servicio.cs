using System;
using System.Collections.Generic;

namespace LECCIONCLIENTES.Models;

public partial class Servicio
{
    public int Ids { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaCreacion { get; set; }
}
