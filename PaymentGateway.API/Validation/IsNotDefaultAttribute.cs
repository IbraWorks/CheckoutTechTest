using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.API.Validation
{
    public class IsNotDefaultAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsDefaultValue(value) ? new ValidationResult($"{validationContext.DisplayName} cannot be a default value") : ValidationResult.Success;
        }

        private static bool IsDefaultValue(object value)
        {
            return value switch
            {
                int i when i == default => true,
                Guid guid when guid == default => true,
                decimal i when i == default => true,
                double i when i == default => true,
                long i when i == default => true,
                Enum e when Convert.ToInt32(e) == 0 => true,
                _ => false
            };
        }
    }
}
