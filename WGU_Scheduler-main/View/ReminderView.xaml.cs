using System.Windows.Controls;

using Scheduler.ViewModel;

namespace Scheduler.View
{
    public partial class ReminderView : UserControl
    {
        public ReminderView()
        {
            this.DataContext = new ReminderViewModel();
            InitializeComponent();
        }
    }
}
