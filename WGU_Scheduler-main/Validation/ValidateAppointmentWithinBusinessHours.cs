using System;
using System.Globalization;
using System.Windows.Controls;

namespace Scheduler.Validation
{
    public class ValidateAppointmentWithinBusinessHours : ValidationRule
    {
        public int StartHour = DateTime.Parse("09:00").Hour;
        public int EndHour = DateTime.Parse("17:00").Hour;

        public ValidateAppointmentWithinBusinessHours()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (!DateTime.TryParse(value.ToString(), out DateTime timeValue))
                {
                    return new ValidationResult(false, $"Not a valid date time value.");
                }

                if (!int.TryParse(timeValue.Hour.ToString(), out int SetHour))
                {
                    return new ValidationResult(false, $"Not valid: {timeValue:H}");
                }

                if ((SetHour < StartHour) || (SetHour >= EndHour))
                {
                    return new ValidationResult(false,
                        $"Appointment is outside business hours, {StartHour} - {EndHour}");
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
