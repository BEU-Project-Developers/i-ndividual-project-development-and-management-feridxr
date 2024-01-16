using System.Windows.Controls;

using Scheduler.ViewModel;

namespace Scheduler.View
{
    public partial class AppointmentView : UserControl
    {
        public AppointmentView()
        {
            this.DataContext = new AppointmentViewModel();
            InitializeComponent();
        }
    }
}
