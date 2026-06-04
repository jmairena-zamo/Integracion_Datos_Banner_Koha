using System.ComponentModel.DataAnnotations;

namespace Integracion_Datos_Banner_Koha.Auth.DTO
{
    public class TokenValidationDto
    {
        [Required]
        public string? Token { get; set; }
    }
}
