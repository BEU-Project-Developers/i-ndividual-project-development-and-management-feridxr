using Scheduler.Model.DBEntities;

using System;

namespace Scheduler.Model
{
    internal class AppointmentModel
    {
        public partial class Appointment
        {
            public int AppointmentId { get; set; }
            public string Contact { get; set; }
            public DateTime CreateDate { get; set; }
            public string CreatedBy { get; set; }
            public virtual Customer Customer { get; set; }
            public int CustomerId { get; set; }
            public string Description { get; set; }
            public DateTime End { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime EndTime { get; set; }
            public DateTime LastUpdate { get; set; }
            public string LastUpdateBy { get; set; }
            public string Location { get; set; }
            public DateTime Start { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime StartTime { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string Url { get; set; }
            public virtual User User { get; set; }
            public int UserId { get; set; }
        }
    }
}