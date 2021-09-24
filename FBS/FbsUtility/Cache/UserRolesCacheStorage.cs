namespace Fbs.Utility.Cache
{
    using System.Web;

    using Fbs.Utility.Interfaces;

    /// <summary>
    /// реализация кэша хранения разрешений пользователя
    /// </summary>
    public class UserRolesCacheStorage : IUserRolesCacheStorage
    {
        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public void Remove(string key)
        {
            HttpContext.Current.Items.Remove(key);
        }

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
        public T Retrieve<T>(string key)
        {
            T itemStored = (T)HttpContext.Current.Items[key];
            if (itemStored == null)
            {
                itemStored = default(T);
            }

            return itemStored;
        }

        /// <summary>
        /// The store.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public void Store(string key, object data)
        {
            HttpContext.Current.Items.Add(key, data);
        }
    }
}