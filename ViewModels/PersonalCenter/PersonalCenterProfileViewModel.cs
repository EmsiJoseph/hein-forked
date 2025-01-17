using System.ComponentModel.DataAnnotations;
using Hein.Models;

namespace Hein.Models.PersonalCenter
{
    public class PersonalCenterProfileViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [Range(typeof(DateOnly), "1900-01-01", "9999-12-31", ErrorMessage = "Please enter a valid date")]
        [CustomValidation(typeof(OnboardingViewModel), nameof(ValidateDateOfBirth))]
        public DateOnly? Dob { get; set; }

        public List<string> StylePreference { get; set; }
        public List<string> ShoppingPreference { get; set; }
        public List<string> FashionStylePreference { get; set; }

        [Required(ErrorMessage = "Please select your gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        
        public bool IsEmailConfirmed { get; set; }


        public string? Phone { get; set; }

        public static ValidationResult ValidateDateOfBirth(DateOnly? dob, ValidationContext context)
        {
            if (!dob.HasValue)
                return new ValidationResult("Date of birth is required.");

            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dob.Value.Year;

            // Adjust age if birthday hasn't occurred this year
            if (dob.Value.DayOfYear > today.DayOfYear)
                age--;

            if (dob.Value > today)
                return new ValidationResult("Date cannot be in the future.");

            if (age < 18)
                return new ValidationResult("You must be at least 18 years old.");


            return ValidationResult.Success;
        }
    }
}