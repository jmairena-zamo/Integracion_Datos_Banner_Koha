using System.ComponentModel.DataAnnotations;

namespace Integracion_Datos_Banner_Koha.Models.DTOs.Auth
{
    public class RefreshTokenInputDto
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }
}
