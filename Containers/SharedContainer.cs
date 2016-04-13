using Microsoft.Practices.Unity;

namespace WpfApplication1.Containers
{
    /// <summary>
    /// The shared DI container.
    /// </summary>
    public static class SharedContainer
    {
        private static IUnityContainer container;
        private static readonly object containerLock = new object();

        /// <summary>
        /// Gets the container.
        /// </summary>
        public static IUnityContainer Container
        {
            get
            {
                if (container == null)
                {
                    lock (containerLock)
                    {
                        if (container == null)
                            container = new UnityContainer();
                    }
                }

                return container;
            }
        }
    }
}
