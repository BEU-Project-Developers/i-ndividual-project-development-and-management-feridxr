using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;

using GalaSoft.MvvmLight.CommandWpf;

using Microsoft.EntityFrameworkCore;

using Scheduler.Exceptions;
using Scheduler.Model.DBEntities;

namespace Scheduler.ViewModel
{
    public class AppointmentViewModel : ViewModelBase
    {
        private bool _addMode = false;
        private bool _editMode = false;
        private bool _viewMode = true;
        private bool _gridDisplay = true;
        private bool _calenderByMonthDisplay = false;
        private bool _calenderByWeekDisplay = false;
        private bool _gridSelected;
        private bool _monthlyCalendarSelected;
        private bool _weeklyCalendarSelected;
        private bool _modifyAppointmentSelected;

        private Appointment _selectedappointment;
        private Customer _selectedcustomer;

        private int _customerIndex;

        private ObservableCollection<Appointment> _allappointmentsloaded;
        private ObservableCollection<Appointment> _weeklyAppointments;
        private ObservableCollection<Appointment> _monthlyAppointments;
        
        private object _tabControlSelectedItem;
        
        private string _selectedMonth = DateTime.Today.ToString("MMMM");
        private string _selectedYear = DateTime.Today.Year.ToString();

        enum Mode
        {
            Add,
            Edit,
            View
        }

        enum Display
        {
            Grid,
            CalenderByMonth,
            CalendarByWeek
        }

        private void SetMode(Mode mode)
        {
            if (mode == Mode.Add)
            {
                AddMode = true;
                EditMode = true;
                ViewMode = false;
                SelectedAppointment = null;
                ModifyAppointmentSelected = true;
            }
            if (mode == Mode.Edit)
            {
                EditMode = true;
                ViewMode = false;
                ModifyAppointmentSelected = true;
            }
            if (mode == Mode.View)
            {
                EditMode = false;
                AddMode = false;
                ViewMode = true;
            }
        }

        private void SetDisplay(Display display)
        {
            if (display == Display.Grid)
            {
                GridDisplay = true;
                CalenderByMonthDisplay = false;
                CalenderByWeekDisplay = false;
            }
            if (display == Display.CalenderByMonth)
            {
                GridDisplay = false;
                CalenderByMonthDisplay = true;
                CalenderByWeekDisplay = false;
            }
            if (display == Display.CalendarByWeek)
            {
                GridDisplay = false;
                CalenderByMonthDisplay = false;
                CalenderByWeekDisplay = true;
            }
        }

        private void OnAddButton(Appointment appointment)
        {
            SetMode(Mode.Add);
            if (appointment == null)
            {
                SelectedAppointment = AllAppointments.FirstOrDefault();
            }
        }

        private void OnEditButton(Appointment appointment)
        {
            SetMode(Mode.Edit);
        }

        private void OnDeleteButton(Appointment appointment)
        {
            if (MessageBox.Show("Are you sure you want to delete this appointment?" +
                    "\r\n Id:" + appointment.AppointmentId +
                    "\r\n Title:" + appointment.Title +
                    "\r\n Location:" + appointment.Location +
                    "\r\n Contact:" + appointment.Contact +
                    "\r\n Start:" + appointment.Start.ToLocalTime().ToString() +
                    "\r\n End:" + appointment.End.ToLocalTime().ToString(),
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var context = new DBContext();
                context.Remove(appointment);
                context.SaveChanges();
                SetMode(Mode.View);
                LoadAppointments();
            }
        }

        private void OnSaveButton(Appointment appointment)
        {
            try
            {
                CheckIfAppointmentOverlapping(appointment);
            } catch(OverlappingAppointmentException e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            var context = new DBContext();
            if (AddMode)
            {
                int NextId = AllAppointments.OrderByDescending(a => a.AppointmentId).FirstOrDefault().AppointmentId + 1;
                Appointment NewAppointment = new Appointment
                {
                    //AppointmentId = NextId,
                    CustomerId = appointment.CustomerId,
                    UserId = appointment.UserId,
                    Title = appointment.Title,
                    Description = appointment.Description,
                    Location = appointment.Location,
                    Contact = appointment.Contact,
                    Type = appointment.Type,
                    Url = appointment.Url,
                    Start = appointment.Start.ToUniversalTime(),
                    End = appointment.End.ToUniversalTime(),
                    CreateDate = appointment.CreateDate.ToUniversalTime(),
                    CreatedBy = appointment.CreatedBy,
                    LastUpdate = appointment.LastUpdate.ToUniversalTime(),
                    LastUpdateBy = appointment.LastUpdateBy
                };

                // Set any string null properties to empty string
                foreach (var propertyInfo in NewAppointment.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        if (propertyInfo.GetValue(NewAppointment, null) == null)
                        {
                            propertyInfo.SetValue(NewAppointment, string.Empty, null);
                        }
                    }
                }

                context.Add(NewAppointment);
            }
            else
            {
                appointment.Start = appointment.Start.ToUniversalTime();
                appointment.End = appointment.End.ToUniversalTime();
                context.Update(appointment);
            }

            context.SaveChanges();
            LoadAppointments();
            SetMode(Mode.View);
        }

        public void CheckIfAppointmentOverlapping(Appointment appointment)
        {
            foreach (Appointment existingAppt in AllAppointments.Where(appt => appt.AppointmentId != appointment.AppointmentId))
            {
                string message =
                    $"The start of the appointment conflicts with\r\n" +
                    "an existing appointment. Please correct the time.\r\n" +
                    $"Existing Appointment: {existingAppt.Start.ToLocalTime()} - {existingAppt.End.ToLocalTime()}";

                DateTime newApptStart = appointment.Start.ToUniversalTime();
                DateTime newApptEnd = appointment.End.ToUniversalTime();
                DateTime existingApptStart = existingAppt.Start.ToUniversalTime();
                DateTime existingApptEnd = existingAppt.End.ToUniversalTime();

                if ((existingApptStart < newApptStart) && (existingApptEnd > newApptStart))
                {
                    throw (new OverlappingAppointmentException(message));
                }
                if ((existingApptStart < newApptEnd) && (existingApptEnd > newApptStart))
                {
                    throw (new OverlappingAppointmentException(message));
                }
            }
        }

        private void OnCancelButton(Appointment appointment)
        {
            LoadAppointments();
            SetMode(Mode.View);
        }

        private void OnGridRadioButton(Appointment appointment)
        {
            SetDisplay(Display.Grid);
        }

        private void OnCalendarByMonthRadioButton(Appointment appointment)
        {
            SetDisplay(Display.CalenderByMonth);
        }

        private void OnCalendarByWeekRadioButton(Appointment appointment)
        {
            SetDisplay(Display.CalendarByWeek);
        }

        public AppointmentViewModel()
        {
            AddAppointmentCommand = new RelayCommand<Appointment>(OnAddButton);
            EditAppointmentCommand = new RelayCommand<Appointment>(OnEditButton);
            DeleteAppointmentCommand = new RelayCommand<Appointment>(OnDeleteButton);

            SaveAppointmentCommand = new RelayCommand<Appointment>(OnSaveButton);
            CancelAppointmentCommand = new RelayCommand<Appointment>(OnCancelButton);

            SetGridCommand = new RelayCommand<Appointment>(OnGridRadioButton);
            SetCalendarByMonthCommand = new RelayCommand<Appointment>(OnCalendarByMonthRadioButton);
            SetCalendarByWeekCommand = new RelayCommand<Appointment>(OnCalendarByWeekRadioButton);
        }

        public RelayCommand<string> GetAppointmentsCommand { get; private set; }
        public RelayCommand<Appointment> AddAppointmentCommand { get; private set; }
        public RelayCommand<Appointment> EditAppointmentCommand { get; private set; }
        public RelayCommand<Appointment> DeleteAppointmentCommand { get; private set; }
        public RelayCommand<Appointment> SaveAppointmentCommand { get; private set; }
        public RelayCommand<Appointment> CancelAppointmentCommand { get; private set; }
        public RelayCommand<Appointment> SetGridCommand { get; private set; }
        public RelayCommand<Appointment> SetCalendarByMonthCommand { get; private set; }
        public RelayCommand<Appointment> SetCalendarByWeekCommand { get; private set; }

        public bool AddMode
        {
            get => _addMode;
            set
            {
                if (value != _addMode)
                {
                    SetProperty(ref _addMode, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool ViewMode
        {
            get => _viewMode;
            set
            {
                if (value != _viewMode)
                {
                    SetProperty(ref _viewMode, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool EditMode
        {
            get => _editMode;
            set
            {
                if (value != _editMode)
                {
                    SetProperty(ref _editMode, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool GridDisplay
        {
            get => _gridDisplay;
            set
            {
                if (value != _gridDisplay)
                {
                    SetProperty(ref _gridDisplay, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool CalenderByMonthDisplay
        {
            get => _calenderByMonthDisplay; 
            set
            {
                if (value != _calenderByMonthDisplay)
                {
                    SetProperty(ref _calenderByMonthDisplay, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool CalenderByWeekDisplay
        {
            get => _calenderByWeekDisplay;
            set
            {
                if (value != _calenderByWeekDisplay)
                {
                    SetProperty(ref _calenderByWeekDisplay, value);
                    OnPropertyChanged();
                }
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

        public ObservableCollection<Appointment> AllAppointmentsLoaded
        {
            get => _allappointmentsloaded;
            set 
            {
                if (value != _allappointmentsloaded)
                {
                    SetProperty(ref _allappointmentsloaded, value);
                    OnPropertyChanged();
                }
            }
        }

        public async void LoadAppointments()
        {
            DBContext context = new DBContext();
            List<Appointment> appointments = await context.Appointment.ToListAsync();
            foreach (Appointment appointment in appointments)
            {
                appointment.Start = appointment.Start.ToLocalTime();
                appointment.End = appointment.End.ToLocalTime();
            }
            AllAppointmentsLoaded = new ObservableCollection<Appointment>(appointments);

            DateTime Now = DateTime.Now.ToLocalTime();

            WeeklyAppointments = new ObservableCollection<Appointment>(
                appointments
                .Where(appt => ISOWeek.GetWeekOfYear(appt.Start) == ISOWeek.GetWeekOfYear(Now))
                .Where(appt => appt.Start.Year == Now.Year)
            );

            MonthlyAppointments = new ObservableCollection<Appointment>(
                appointments
                .Where(appt => appt.Start.Month == Now.Month)
                .Where(appt => appt.Start.Year == Now.Year)
            );

        }

        public ObservableCollection<Appointment> WeeklyAppointments
        {
            get => _weeklyAppointments;
            set
            {
                if (value != _weeklyAppointments)
                {
                    SetProperty(ref _weeklyAppointments, value);
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Appointment> MonthlyAppointments
        {
            get => _monthlyAppointments; set
            {
                if (value != _monthlyAppointments)
                {
                    SetProperty(ref _monthlyAppointments, value);
                    OnPropertyChanged();
                }
            }
        }

        public Appointment SelectedAppointment
        {
            get => _selectedappointment;
            set
            {
                if (value != null && value != _selectedappointment)
                {
                    value.Start = value.Start.ToLocalTime();
                    value.End = value.End.ToLocalTime();
                    
                    SetProperty(ref _selectedappointment, value);
                    OnPropertyChanged();

                    DBContext context = new DBContext();
                    SelectedCustomer = context.Customer.Find(value.CustomerId);
                }

                
            }
        }

        public Customer SelectedCustomer
        {
            get => _selectedcustomer;
            set
            {
                if (value != null && value != _selectedcustomer)
                {
                    SetProperty(ref _selectedcustomer, value);
                    OnPropertyChanged();

                    if (SelectedAppointment != null && SelectedAppointment.CustomerId != value.CustomerId)
                    {
                        SelectedAppointment.CustomerId = value.CustomerId;
                    }
                }
            }
        }

        public ObservableCollection<Customer> AllCustomers
        {
            get
            {
                DBContext context = new DBContext();
                return new ObservableCollection<Customer>(context.Customer.ToList());
            }
            set { }
        }

        public int CustomerIndex
        { 
            get => _customerIndex;
            set
            {
                if (value != _customerIndex)
                {
                    SetProperty(ref _customerIndex, value);
                    OnPropertyChanged();
                }
            }
        }

        public object TabControlSelectedItem
        {
            get => _tabControlSelectedItem;
            set
            {
                if (value != _tabControlSelectedItem)
                {
                    SetProperty(ref _tabControlSelectedItem, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool GridSelected
        {
            get => _gridSelected;
            set
            {
                if (value != _gridSelected)
                {
                    SetProperty(ref _gridSelected, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool MonthlyCalendarSelected
        {
            get => _monthlyCalendarSelected;
            set
            {
                if (value != _monthlyCalendarSelected)
                {
                    SetProperty(ref _monthlyCalendarSelected, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool WeeklyCalendarSelected
        {
            get => _weeklyCalendarSelected;
            set
            {
                if (value != _weeklyCalendarSelected)
                {
                    SetProperty(ref _weeklyCalendarSelected, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool ModifyAppointmentSelected
        {
            get => _modifyAppointmentSelected; 
            set
            {
                if (value != _modifyAppointmentSelected)
                {
                    SetProperty(ref _modifyAppointmentSelected, value);
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedMonth
        {
            get => _selectedMonth; 
            set
            {
                if (value != _selectedMonth)
                {
                    SetProperty(ref _selectedMonth, value);
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedYear
        {
            get => _selectedYear; 
            set
            {
                if (value != _selectedYear)
                {
                    SetProperty(ref _selectedYear, value);
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Months
        {
            get
            {
                return new ObservableCollection<string>()
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December"
                };
            }
            set { }
        }

        public ObservableCollection<string> Years
        {
            get
            {
                List<string> years = new List<string>();
                for (int i = -3; i < 3; i++)
                {
                    years.Add(
                        DateTime.Today.AddYears(i).Year.ToString()
                    );
                }

                return new ObservableCollection<string>(years);
            }
            set { }
        }

    }
}
