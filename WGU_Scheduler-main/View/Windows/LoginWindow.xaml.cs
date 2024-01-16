using System.Windows;

using Scheduler.ViewModel;

namespace Scheduler.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            this.DataContext = new LoginWindowViewModel();
            InitializeComponent();
        }
    }
}