using System.ComponentModel.DataAnnotations;

namespace ApiBase.Auth.DTO
{
    public class ApiKeyValidationDto
    {
        [Required]
        public string? ApiKey { get; set; }
        [Required]
        public string[]? Roles { get; set; }
    }
}
