namespace Esrp.Utility.Interfaces
{
    /// <summary>
    /// базовый интерфейс для реализаций кэша
    /// </summary>
    public interface ICacheStorage
    {
        #region Public Methods

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        void Remove(string key);

        /// <summary>
        /// The retrieve.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="T">
        /// тип обьекта
        /// </typeparam>
        /// <returns>
        /// инстанс обьекта
        /// </returns>
        T Retrieve<T>(string key);

        /// <summary>
        /// The store.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        void Store(string key, object data);

        #endregion
    }
}