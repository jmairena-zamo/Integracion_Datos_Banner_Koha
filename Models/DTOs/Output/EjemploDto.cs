using System.ComponentModel.DataAnnotations;

namespace ApiBase.Models.DTOs.Output
{
    public class EjemploDto
    {
        public int? Id { get; set; }
        public int? CodigoEstudiante { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Telefono { get; set; }
    }
}
