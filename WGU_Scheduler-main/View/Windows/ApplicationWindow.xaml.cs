using System.Windows;

using Scheduler.ViewModel;

namespace Scheduler.View
{
    public partial class ApplicationWindow : Window
    {
        public ApplicationWindow()
        {
            this.DataContext = new ApplicationWindowViewModel();
            InitializeComponent();
        }
    }
}
