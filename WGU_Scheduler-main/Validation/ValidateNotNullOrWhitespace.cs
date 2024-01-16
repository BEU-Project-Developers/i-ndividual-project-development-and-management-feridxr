using System;
using System.Globalization;
using System.Windows.Controls;

namespace Scheduler.Validation
{
    public class ValidateNotNullOrWhitespace : ValidationRule
    {
        public ValidateNotNullOrWhitespace()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return new ValidationResult(false, $"Must have a value");
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"{e.Message}");
            }

            return ValidationResult.ValidResult;
        }
    }
}
