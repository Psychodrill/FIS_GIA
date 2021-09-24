namespace Ege.Check.Dal.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Common.Extensions;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using global::Common.Logging;
    using JetBrains.Annotations;

    class CacheLockAcquirer : ICacheLockAcquirer
    {
        [NotNull]
        private static readonly ILog Logger = LogManager.GetLogger<CacheLockAcquirer>();

        public bool TryLockGet<T>(
            ICacheWrapper cache, 
            string key, 
            int maxLockDuration,
            out T result,
            out ICacheLockWrapper lockHandle)
        {
            lockHandle = null;
            try
            {
                result = cache.GetAndLock<T>(key, TimeSpan.FromMilliseconds(maxLockDuration), out lockHandle).As<T>();
                return true;
            }
            catch (LivingCacheException ex)
            {
                Logger.TraceFormat("Lock not acquired for {0}: {1}", key, ex);
                result = default(T);
                return false;
            }
        }

        public async Task<KeyValuePair<T, ICacheLockWrapper>> LockGet<T>(
            ICacheWrapper cache,
            string key,
            int maxLockDuration,
            int waitInterval)
        {
            T cached;
            ICacheLockWrapper lockHandle;
            while (!TryLockGet(cache, key, maxLockDuration, out cached, out lockHandle))
            {
                Logger.TraceFormat("Lock not acquired for key {0}", key);
                await Task.Delay(waitInterval);
            }
            return new KeyValuePair<T, ICacheLockWrapper>(cached, lockHandle);
        }


        public async Task<KeyValuePair<IEnumerable<T>, ICacheLockWrapper>> LockBulkGet<T>(ICacheWrapper cache, string lockKey, IEnumerable<string> keys, int maxLockDuration, int waitInterval)
        {
            var cacheLock = await LockGet<string>(cache, lockKey, maxLockDuration, waitInterval);
            var values = await cache.BulkGet<T>(keys);
            cache.ExtendLock(lockKey, maxLockDuration, cacheLock.Value);
            return new KeyValuePair<IEnumerable<T>, ICacheLockWrapper>(values, cacheLock.Value);
        }
    }
}
