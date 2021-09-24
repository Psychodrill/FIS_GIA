namespace Fbs.Utility.IoC
{
    using Fbs.Utility.Cache;
    using Fbs.Utility.Interfaces;

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
            container.RegisterType<IUserRolesCacheStorage, UserRolesCacheStorage>(
                new ApplicationLifetimeManager<UserRolesCacheStorage>());
        }
    }
}