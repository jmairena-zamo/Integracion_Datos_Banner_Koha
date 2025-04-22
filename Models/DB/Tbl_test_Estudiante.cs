using System;
using System.Collections.Generic;

namespace ApiBase.Models.DB;

public partial class Tbl_test_Estudiante
{
    public int Id { get; set; }

    public string CodigoEstudiante { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefono { get; set; }

    public string CodigoEstado { get; set; } = null!;

    public string UsuarioCreador { get; set; } = null!;

    public DateTime FechaCreador { get; set; }

    public string? UsuarioModifica { get; set; }

    public DateTime? FechaModifica { get; set; }
}
