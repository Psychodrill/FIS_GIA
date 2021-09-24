namespace Ege.Check.Dal.Cache.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;

    public interface ICacheLockAcquirer
    {
        /// <summary>
        ///     Попытаться получить лок на элемент кэша
        /// </summary>
        /// <param name="cache">Кэш</param>
        /// <param name="key">Ключ элемента</param>
        /// <param name="maxLockDuration">Время в миллисекундах, на которое берётся лок</param>
        /// <param name="result">Текущее содержимое кэша</param>
        /// <param name="lockHandle">Лок</param>
        /// <returns>Успешность взятия лока</returns>
        bool TryLockGet<T>([NotNull] ICacheWrapper cache, string key, int maxLockDuration, out T result, out ICacheLockWrapper lockHandle);

        /// <summary>
        /// Получить лок на элемент кэша
        /// </summary>
        /// <param name="cache">Кэш</param>
        /// <param name="key">Ключ элемента</param>
        /// <param name="maxLockDuration">Время в миллисекундах, на которое берётся лок</param>
        /// <param name="waitInterval">Интервал ожидания после неудачной попытки взять лок</param>
        /// <returns>Объект из кэша + лок</returns>
        [NotNull]
        Task<KeyValuePair<T, ICacheLockWrapper>> LockGet<T>(
            [NotNull]ICacheWrapper cache,
            [NotNull]string key,
            int maxLockDuration,
            int waitInterval);

        /// <summary>
        /// Получив лок на кэш, извлечь элементы
        /// </summary>
        /// <typeparam name="T">Тип элементов</typeparam>
        /// <param name="cache">Кэш</param>
        /// <param name="lockKey">Ключ лока</param>
        /// <param name="keys">Ключи получаемых объектов</param>
        /// <param name="maxLockDuration">Время в миллисекундах, на которое берётся лок</param>
        /// <param name="waitInterval">Интервал ожидания после неудачной попытки взять лок</param>
        /// <returns>Объекты из кэша + лок</returns>
        [NotNull]
        Task<KeyValuePair<IEnumerable<T>, ICacheLockWrapper>> LockBulkGet<T>(
            [NotNull]ICacheWrapper cache,
            [NotNull]string lockKey, 
            [NotNull]IEnumerable<string> keys,
            int maxLockDuration,
            int waitInterval);
    }
}
