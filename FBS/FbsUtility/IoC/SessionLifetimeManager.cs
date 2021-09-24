namespace Fbs.Utility.IoC
{
    using System;
    using System.Web;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Мэнеджер "жизни" обьектов уровня сессии
    /// </summary>
    /// <typeparam name="T">
    /// тип обьекта
    /// </typeparam>
    public class SessionLifetimeManager<T> : LifetimeManager, IDisposable
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
            return HttpContext.Current.Session[typeof(T).AssemblyQualifiedName];
        }

        /// <summary>
        /// The remove value.
        /// </summary>
        public override void RemoveValue()
        {
            HttpContext.Current.Session.Remove(typeof(T).AssemblyQualifiedName);
        }

        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        public override void SetValue(object newValue)
        {
            HttpContext.Current.Session[typeof(T).AssemblyQualifiedName] = newValue;
        }

        #endregion
    }
}