using System.ComponentModel.DataAnnotations;
using Hein.Data.Validation;

namespace Hein.Models
{
    public class SignInOrRegisterViewModel
    {
        
        [Required(ErrorMessage = "Please enter your email or mobile number.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; } = "+63";
        public string ActiveField { get; set; } = "email"; // To track which field is being used
    }
}
