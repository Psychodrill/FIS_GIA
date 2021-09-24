namespace Ege.Check.Dal.Cache.AppFabric
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Common.Extensions;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;
    using Microsoft.ApplicationServer.Caching;

    internal class AppFabricCacheWrapper : CacheWrapperBase<DataCacheException>, ICacheWrapper
    {
        [NotNull] private readonly DataCache _cache;

        public AppFabricCacheWrapper([NotNull] DataCache cache)
        {
            _cache = cache;
        }

        internal DataCache CacheObject
        {
            get { return _cache; }
        }

        public T Get<T>(string key)
        {
            return ExecuteAndWrapException(() => _cache.Get(key).As<T>());
        }

        public void Put(string key, object item)
        {
            ExecuteAndWrapException(() =>
                {
                    if (item == null)
                    {
                        _cache.Remove(key);
                    }
                    else
                    {
                        _cache.Put(key, item);
                    }
                });
        }

        public T GetAndLock<T>(string key, TimeSpan lockDuration, out ICacheLockWrapper handle)
        {
            DataCacheLockWrapper lockHandle = null;
            var cachedObject = ExecuteAndWrapException(() =>
                {
                    DataCacheLockHandle appFabricHandle;
                    var result = _cache.GetAndLock(key, lockDuration, out appFabricHandle, true).As<T>();
                    lockHandle = new DataCacheLockWrapper { Handle = appFabricHandle };
                    return result;
                });
            handle = lockHandle;
            return cachedObject;
        }

        public void PutAndUnlock(string key, object item, ICacheLockWrapper handle)
        {
            ExecuteAndWrapException(() =>
                {
                    var appFabricHandle = (DataCacheLockWrapper) handle;
                    _cache.PutAndUnlock(key, item, appFabricHandle.Handle);
                });
        }

        public override bool IsLivingCacheException(DataCacheException exception)
        {
            return exception.ErrorCode == DataCacheErrorCode.ObjectLocked
                   || exception.ErrorCode == DataCacheErrorCode.Timeout;
        }

        public bool SupportsBulkOperations
        {
            get { return false; }
        }

        public Task<IEnumerable<T>> BulkGet<T>(IEnumerable<string> keys)
        {
            throw new NotSupportedException();
        }

        public Task BulkPut<T>(IEnumerable<KeyValuePair<string, T>> keyValuePairs)
        {
            throw new NotSupportedException();
        }


        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        public Task PutAsync(string key, object item)
        {
            Put(key, item);
            return Task.FromResult(0);
        }

        public void ExtendLock(string lockKey, int maxLockDuration, ICacheLockWrapper lockWrapper)
        {
            throw new NotSupportedException();
        }

        public Task Clear()
        {
            Parallel.ForEach(
                _cache.GetSystemRegions(), 
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, 
                region => _cache.ClearRegion(region));
            return Task.FromResult(0);
        }
    }
}
