namespace Ege.Check.Dal.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;

    public abstract class BaseLockableCache<TCached, TKey> : BaseCache<TCached, TKey>, ILockableCache<TKey, TCached> 
        where TCached: class, new()
    {
        [NotNull] private readonly ICacheLockAcquirer _lockAcquirer;

        private readonly int _waitMilliseconds;
        private readonly int _maxLockDuration;

        protected BaseLockableCache(
            [NotNull] ICacheFailureHelper failureHelper, 
            [NotNull] ICacheSettingsProvider settings,
            [NotNull] ICacheLockAcquirer lockAcquirer)
            : base(failureHelper)
        {
            _lockAcquirer = lockAcquirer;
            _waitMilliseconds = settings.GetCacheWaitLockQuantum();
            _maxLockDuration = settings.GetCacheMaxLockDuration();
        }

        /// <summary>
        ///     Получить элемент, дождавшись взятия лока на элемент кэша
        /// </summary>
        /// <param name="cacheWrapper">Кэш</param>
        /// <param name="key">Ключ элемента</param>
        /// <param name="createIfNotExists">Создавать ли объект в кэше, если его там ещё нет</param>
        /// <returns>Текущее содержимое кэша, взятый лок</returns>
        /// <exception cref="CacheException">Кэш лежит</exception>
        public async Task<CacheLock<TCached>> LockGet(ICacheWrapper cacheWrapper, TKey key, bool createIfNotExists = true)
        {
            if (cacheWrapper == null)
            {
                throw new ArgumentNullException("cacheWrapper");
            }
            var keyString = GetKeyString(key);
            Logger.TraceFormat("LockGet {0} started", keyString);
            var locked = await _lockAcquirer.LockGet<TCached>(cacheWrapper, keyString, _maxLockDuration, _waitMilliseconds);
            Logger.TraceFormat("LockGet {0} finished (handle {1}, thread {2})", keyString, locked.Value, System.Threading.Thread.CurrentThread.ManagedThreadId);
            var element = createIfNotExists ? (locked.Key ?? new TCached()) : locked.Key;
            return new CacheLock<TCached>
            {
                Element = element,
                Lock = locked.Value,
                CacheWrapper = cacheWrapper,
                Key = keyString,
            };
        }

        public async Task<CacheBulkLock<TKey, TCached>> LockBulkGet(ICacheWrapper cacheWrapper, IEnumerable<TKey> keys, bool createIfNotExists = true)
        {
            var keyValueCollection = keys.Select(key => new CacheBulkElement<TKey, TCached> {Key = key, KeyString = GetKeyString(key)}).ToList();
            Logger.TraceFormat("LockBulkGet started with {0} keys", keyValueCollection.Count);
            var lockKey = GetType().Name;
            var locked = await _lockAcquirer.LockBulkGet<TCached>(cacheWrapper, lockKey, keyValueCollection.Select(kv => kv.KeyString), _maxLockDuration, _waitMilliseconds);
            Logger.TraceFormat("LockBulkGet finished (handle {0}, thread {1})", locked.Value, System.Threading.Thread.CurrentThread.ManagedThreadId);
            if (locked.Key == null)
            {
                throw new InvalidOperationException("ILockAcquirer::LockBulkGet produced null instead of a enumeration of values");
            }
            int index = 0;
            if (createIfNotExists)
            {
                foreach (var value in locked.Key)
                {
                    keyValueCollection[index++].Value = value ?? new TCached();
                }
            }
            else
            {
                foreach (var value in locked.Key)
                {
                    keyValueCollection[index++].Value = value;
                }
            }
            return new CacheBulkLock<TKey, TCached>
            {
                CacheWrapper = cacheWrapper,
                Lock = locked.Value,
                LockKey = lockKey,
                Elements = keyValueCollection,
            };
        }
    }
}
