using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Handles the application startup procedure.
        /// </summary>
        /// <param name="e">The startup event arguments.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Initialize dispatcher helper
            DispatcherHelper.Initialize();

            base.OnStartup(e);
        }
    }
}
