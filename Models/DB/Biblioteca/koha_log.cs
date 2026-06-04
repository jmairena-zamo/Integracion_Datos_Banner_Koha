using System;
using System.Collections.Generic;

namespace Integracion_Datos_Banner_Koha.Models.DB.Biblioteca;

public partial class koha_log
{
    public int id { get; set; }

    public string Codigo_Estudiante { get; set; } = null!;

    public string? Correo_estudiante { get; set; }

    public string? banner_JSON { get; set; }

    public string? Usuario_Crea { get; set; }

    public string? Usuario_Modifica { get; set; }

    public DateTime? Fecha_Creacion { get; set; }

    public DateTime? Fecha_Modifica { get; set; }

    public string? Host { get; set; }
}
