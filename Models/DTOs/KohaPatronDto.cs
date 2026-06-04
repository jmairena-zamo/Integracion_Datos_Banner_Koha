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
