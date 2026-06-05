//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se guardan los datos que provienen de
  la información extradída de Koha con respecto a los perfiles de 
  estudiantes para compararlos con la información de Banner.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

using System.Text.Json.Serialization;

namespace Integracion_Datos_Banner_Koha.Models.DTOs
{
    public class KohaPatronDto
    {
        [JsonPropertyName("firstname")]
        public string? Firstname { get; set; }

        [JsonPropertyName("surname")]
        public string? Surname { get; set; }

        [JsonPropertyName("cardnumber")]
        public string? Cardnumber { get; set; }

        [JsonPropertyName("library_id")]
        public string? LibraryId { get; set; } = "01";

        [JsonPropertyName("category_id")]
        public string? CategoryId { get; set; } = "ST";

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("expired")]
        public bool Expired { get; set; }
    }
}
