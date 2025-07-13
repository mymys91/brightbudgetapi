using System.ComponentModel.DataAnnotations;

using BrightBudgetApp.Core.Attributes;

namespace BrightBudget.API.Models
{
    public class RegisterRequest
    {
        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8), NoSpacesAttribute(ErrorMessage = "Password cannot contain spaces.")]
        public string Password { get; set; } = string.Empty;
    }
}