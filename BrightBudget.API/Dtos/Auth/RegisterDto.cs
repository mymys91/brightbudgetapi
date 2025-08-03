using System.ComponentModel.DataAnnotations;

namespace BrightBudget.API.Dtos.Auth
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}