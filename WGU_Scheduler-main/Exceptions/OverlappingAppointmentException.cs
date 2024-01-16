using System;

namespace Scheduler.Exceptions
{
    public class OverlappingAppointmentException : Exception
    {
        public OverlappingAppointmentException(string message) : base(message)
        {
        }
    }
}