using System.ComponentModel.DataAnnotations;

namespace ApiBase.Models.DTOs.Auth.Input
{
    public class RefreshTokenInputDto
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }
}
