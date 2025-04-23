using System.ComponentModel.DataAnnotations;

namespace ApiBase.Models.DTOs.Auth.Input
{
    public class UserLoginInputDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
