using System.Windows.Controls;

using Scheduler.ViewModel;

namespace Scheduler.View
{
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            this.DataContext = new CustomerViewModel();
            InitializeComponent();
        }
    }
}
