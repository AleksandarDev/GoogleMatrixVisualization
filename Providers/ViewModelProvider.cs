using WpfApplication1.Containers;
using WpfApplication1.ViewModels;
using Microsoft.Practices.Unity;

namespace WpfApplication1.Providers
{
    /// <summary>
    /// The view model provider.
    /// </summary>
    public static class ViewModelProvider
    {
        /// <summary>
        /// Gets the main view model.
        /// </summary>
        public static MainViewModel MainViewModel
        {
            get
            {
                return SharedContainer.Container.Resolve<MainViewModel>();
            }
        }
    }
}
