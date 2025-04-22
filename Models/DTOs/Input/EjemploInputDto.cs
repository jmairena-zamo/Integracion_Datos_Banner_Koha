using System.ComponentModel.DataAnnotations;

namespace ApiBase.Models.DTOs.Input
{
    public class EjemploInputDto
    {
        [Required]
        public int? CodigoEstudiante { get; set; }
        [Required]
        public string? Nombres { get; set; }
        [Required]
        public string? Apellidos { get; set; }
        public string? Telefono { get; set; }
    }
}
