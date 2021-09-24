namespace Fbs.Utility.IoC
{
    using System;
    using System.Web;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Мэнеджер "жизни" обьектов уровня приложения
    /// </summary>
    /// <typeparam name="T">
    /// тип обьекта
    /// </typeparam>
    public class ApplicationLifetimeManager<T> : LifetimeManager, IDisposable
    {
        #region Public Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.RemoveValue();
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <returns>
        /// The get value.
        /// </returns>
        public override object GetValue()
        {
            return HttpRuntime.Cache[typeof(T).AssemblyQualifiedName];
        }

        /// <summary>
        /// The remove value.
        /// </summary>
        public override void RemoveValue()
        {
            HttpRuntime.Cache.Remove(typeof(T).AssemblyQualifiedName);
        }

        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        public override void SetValue(object newValue)
        {
            HttpRuntime.Cache[typeof(T).AssemblyQualifiedName] = newValue;
        }

        #endregion
    }
}