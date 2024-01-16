using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Scheduler.Model.DBEntities
{
    public partial class User
    {
        public User()
        {
            Appointment = new HashSet<Appointment>();
        }

        public sbyte Active { get; set; }
        public virtual ICollection<Appointment> Appointment { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}