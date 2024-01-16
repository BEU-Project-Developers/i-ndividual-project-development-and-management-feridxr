using System.Windows.Controls;

using Scheduler.ViewModel;

namespace Scheduler.View
{
    public partial class ReportView : UserControl
    {
        public ReportView()
        {
            this.DataContext = new ReportViewModel();
            InitializeComponent();
        }

    }
}
