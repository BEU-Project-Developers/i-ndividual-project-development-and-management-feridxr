using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Scheduler.Model.DBEntities
{
    public partial class Customer
    {
        public Customer()
        {
            Appointment = new HashSet<Appointment>();
        }

        public sbyte Active { get; set; }
        public virtual Address Address { get; set; }
        public int AddressId { get; set; }
        public virtual ICollection<Appointment> Appointment { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }
    }
}