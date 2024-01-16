using Scheduler.View;

using System.Globalization;
using System.Threading;
using System.Windows;

namespace Scheduler
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SetLocalization();
            Window AppWindow = new LoginWindow();
            AppWindow.Show();
        }

        public void SetLocalization()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture.Name switch
            {
                "ja-JP" => new System.Globalization.CultureInfo("ja-JP"),
                _ => new System.Globalization.CultureInfo("en-US"),
            };
        }
    }
}
