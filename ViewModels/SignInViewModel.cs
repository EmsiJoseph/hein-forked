using System.ComponentModel.DataAnnotations;
using Hein.Data.Validation;

namespace Hein.Models

{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; } = false;
        
        public bool EmailSubscription { get; set; } = false;
    }
}
