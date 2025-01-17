using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Hein.Data.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EmailOrPhoneAttribute : ValidationAttribute
    {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string PhonePattern = @"^\+?\d{7,15}$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Please enter your email or mobile number.");
            }

            var input = value.ToString().Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult("Please enter your email or mobile number.");
            }

            bool isEmail = Regex.IsMatch(input, EmailPattern);
            bool isPhone = Regex.IsMatch(input, PhonePattern);

            if (!isEmail && !isPhone)
            {
                return new ValidationResult("Please enter a valid email address or mobile number.");
            }

            return ValidationResult.Success;
        }
    }
}