using System;

namespace Scheduler.Model
{
    public class ConsultantReportModel
    {
        public DateTime Appointment { get; set; }
        public string AppointmentType { get; set; }
        public string Consultant { get; set; }
        public string CustomerName { get; set; }
    }

    public class MonthlyReportModel
    {
        public string AppointmentType { get; set; }
        public int AppointmentTypeCount { get; set; }
        public string Month { get; set; }
    }

    public class ReportModel
    {
    }
}