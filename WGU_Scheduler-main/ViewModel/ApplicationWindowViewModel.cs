using GalaSoft.MvvmLight.Command;

namespace Scheduler.ViewModel
{

    public class ApplicationWindowViewModel : ViewModelBase
    {
        private ReminderViewModel _reminderViewModel = new ReminderViewModel();
        private AppointmentViewModel _appointmentViewModel = new AppointmentViewModel();
        private CustomerViewModel _customerViewModel = new CustomerViewModel();
        private ReportViewModel _reportViewModel = new ReportViewModel();

        private ViewModelBase _currentViewModel;

        public ApplicationWindowViewModel()
        {
            NavCommand = new RelayCommand<string>(OnNav);
            CurrentViewModel = _reminderViewModel;
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (value != _currentViewModel)
                {
                    SetProperty(ref _currentViewModel, value);
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "Appointment":
                    CurrentViewModel = _appointmentViewModel;
                    break;
                case "Customer":
                    CurrentViewModel = _customerViewModel;
                    break;
                case "Report":
                    CurrentViewModel = _reportViewModel;
                    break;
            }
        }

    }
}