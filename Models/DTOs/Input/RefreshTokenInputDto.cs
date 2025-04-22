using System.ComponentModel.DataAnnotations;

namespace ApiBase.Models.DTOs.Input
{
    public class RefreshTokenInputDto
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }
}
