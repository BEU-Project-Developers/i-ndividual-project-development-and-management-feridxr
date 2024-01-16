using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Scheduler.Model.DBEntities;

namespace Scheduler.ViewModel
{
    public class ReminderViewModel : ViewModelBase
    {
        private string _reminderText;

        private Appointment _currentAppointment;

        public ReminderViewModel()
        {
            GenerateReminder();
        }

        public void GenerateReminder()
        {
            var Now = DateTime.Now.ToLocalTime();

            // Lambda here is prefered, as it reduces the complexity of multiple if 
            // statements and improves readability.
            var remindAppointments = AllAppointments
                .Where(appt => appt.Start.AddMinutes(-15) <= Now)
                .Where(appt => appt.End >= Now);

            if (remindAppointments.Count() > 0)
            {
                foreach (Appointment remindAppointment in remindAppointments)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Title: {remindAppointment.Title}" );
                    sb.AppendLine($"Start Time: {remindAppointment.Start}" );
                    sb.AppendLine($"End Time: {remindAppointment.End}" );
                    sb.AppendLine($"Type: {remindAppointment.Type}" );
                    ReminderText = sb.ToString();
                }
            }
            else
            {
                ReminderText = "You have no appointments within the next 15 minutes.";
            }

        }

        public List<Appointment> AllAppointments
        {
            get
            {
                var context = new DBContext();
                List<Appointment> appointments = context.Appointment.ToList();
                foreach (Appointment appointment in appointments)
                {
                    appointment.Start = appointment.Start.ToLocalTime();
                    appointment.End = appointment.End.ToLocalTime();
                }

                return appointments;
            }
            set
            {
                var context = new DBContext();
                context.Appointment.UpdateRange(value.ToList());
                context.SaveChanges();
            }
        }

        public string ReminderText
        {
            get => _reminderText;
            set
            {
                if (value != _reminderText)
                {
                    SetProperty(ref _reminderText, value);
                    OnPropertyChanged();
                }
            }
        }

        public Appointment CurrentAppointment
        {
            get => _currentAppointment;
            set
            {
                if (value != _currentAppointment)
                {
                    SetProperty(ref _currentAppointment, value);
                    OnPropertyChanged();
                }
            }
        }
    }
}