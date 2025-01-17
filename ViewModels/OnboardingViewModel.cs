using System;
using System.ComponentModel.DataAnnotations;

namespace Hein.Models
{
    public class OnboardingViewModel
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

        [Required(ErrorMessage = "Select at least 5 style preferences.")]
        public string StylePreference { get; set; }

        [Required(ErrorMessage = "Please select your gender")]
        public string Gender { get; set; }

        // Optional coordinates
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

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