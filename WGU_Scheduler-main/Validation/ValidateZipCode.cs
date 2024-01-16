using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Scheduler.Validation
{
    public class ValidateZipCode : ValidationRule
    {
        public ValidateZipCode()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                bool IsMatch = Regex.IsMatch(
                    value.ToString(),
                    @"^\s*(\d{5}|(\d{5}-\d{4}))\s*$"
                );
                if (!IsMatch)
                {
                    return new ValidationResult(false, 
                        $"Invalid zip. Valid zips will be either\r\n" +
                        "5 digits, or 5 digits, a - , and then 4 digits.\r\n" +
                        "Valid Example: 12345\r\n" +
                        "Valid Example: 12345-1234"
                    );
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
