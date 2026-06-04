using System.Text.Json.Serialization;

namespace Integracion_Datos_Banner_Koha.Models.DTOs
{
    public class BannerStudentDto
    {
        [JsonPropertyName("BannerId")]
        public string? BannerId { get; set; }

        [JsonPropertyName("Nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("Apellido")]
        public string? Apellido { get; set; }

        [JsonPropertyName("Correo")]
        public string? Correo { get; set; }

        [JsonPropertyName("Estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("Grado")]
        public string? Grado { get; set; }
    }
}