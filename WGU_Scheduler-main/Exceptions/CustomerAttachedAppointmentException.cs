using System;

namespace Scheduler.Exceptions
{
    public class CustomerAttachedAppointmentException : Exception
    {
        public CustomerAttachedAppointmentException(string message) : base(message)
        {
        }
    }
}