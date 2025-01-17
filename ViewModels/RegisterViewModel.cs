using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Hein.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?\"":{}|<>])[A-Za-z\d!@#$%^&*(),.?\"":{}|<>]{8,}$",
            ErrorMessage =
                "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public bool EmailSubscription { get; set; } = false;
        
        public bool FromOAuth { get; set; } = false;
    }
}