//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se guardan los datos que provienen de
  la información extradída de Banner con respecto a los estudiantes para
  compararlos con los perfiles de estudiantes en Koha.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

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