using System.ComponentModel.DataAnnotations;

namespace auth.Models.DTOs.Input
{
    public class ApiKeyValidationDto
    {
        [Required]
        public string? ApiKey { get; set; }
    }
}
