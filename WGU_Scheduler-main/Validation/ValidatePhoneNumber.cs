using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Scheduler.Validation
{
    public class ValidatePhoneNumber : ValidationRule
    {
        public ValidatePhoneNumber()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                bool IsMatch = Regex.IsMatch(
                    value.ToString(),
                    @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"
                );
                if (!IsMatch)
                {
                    return new ValidationResult(false,
                        $"Invalid Phone number. Valid phone # should be 10 digits.\r\n" +
                        "Valid Example: 1234567891\r\n" +
                        "Valid Example: (123) 456-7891\r\n" +
                        "Valid Example: 123-456-7891"
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
