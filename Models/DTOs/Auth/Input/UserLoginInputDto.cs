using System.ComponentModel.DataAnnotations;

namespace Integracion_Datos_Banner_Koha.Models.DTOs.Auth.Input
{
    public class UserLoginInputDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
