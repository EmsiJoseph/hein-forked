namespace Hein.Models.PersonalCenter
{
    public class ManageAccountViewModel
    {
        public string Email { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Phone { get; set; }
    }
}
