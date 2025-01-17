using System.ComponentModel.DataAnnotations;
 
namespace Hein.Models
{
    public class LoginWith2faViewModel
    {
        
        [Required(ErrorMessage = "Please enter your 2FA code.")]
        public string TwoFactorCode { get; set; }
        
        public bool RememberMachine { get; set; }
        
        public bool RememberMe { get; set; }
    }
}