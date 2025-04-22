using System.ComponentModel.DataAnnotations;

namespace ApiBase.Auth.DTO
{
    public class TokenValidationDto
    {
        [Required]
        public string? Token { get; set; }
    }
}
