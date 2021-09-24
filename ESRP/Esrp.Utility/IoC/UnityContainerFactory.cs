namespace Esrp.Utility.IoC
{
    using Esrp.Utility.Cache;
    using Esrp.Utility.Interfaces;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// фабрика IoC контейнера
    /// </summary>
    public class UnityContainerFactory
    {
        /// <summary>
        /// построить сконфигурированный контейнер
        /// </summary>
        /// <returns>
        /// контейнер
        /// </returns>
        public IUnityContainer CreateConfiguredContainer()
        {
            var container = new UnityContainer();
            this.BuilContainer(container);
            return container;
        }

        private void BuilContainer(UnityContainer container)
        {
            container.RegisterType<IUserDataCacheStorage, UserDataCacheStorage>(
                new ApplicationLifetimeManager<UserDataCacheStorage>());
        }
    }
}