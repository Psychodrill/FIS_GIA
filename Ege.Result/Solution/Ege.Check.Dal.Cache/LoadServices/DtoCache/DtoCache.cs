namespace Ege.Check.Dal.Cache.LoadServices.DtoCache
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;

    class DtoCache<T> : SafeCache, IDtoCache<T>
    {
        [NotNull] private readonly ICacheFailureHelper _failureHelper;
        [NotNull] private readonly ICacheLockAcquirer _acquirer;

        [NotNull] private readonly string _type;
        [NotNull]private readonly string _typeCount;

        public DtoCache(
            [NotNull] ICacheFailureHelper failureHelper,
            [NotNull] ICacheLockAcquirer acquirer) 
            : base(failureHelper)
        {
            _failureHelper = failureHelper;
            _acquirer = acquirer;
            _type = typeof(T).Name;
            _typeCount = string.Format("{0}Count", _type);
        }

        protected string GetKeyString(int key)
        {
            return string.Format("{0}.{1}", _type, key);
        }

        public async Task<KeyValuePair<int, ICacheLockWrapper>> LockGetCount(ICacheWrapper cacheWrapper)
        {
            var result = await _acquirer.LockGet<int?>(cacheWrapper, _typeCount, 5000, 100);
            return new KeyValuePair<int, ICacheLockWrapper>(result.Key ?? 0, result.Value);
        }

        public void UnlockPutCount(ICacheWrapper cacheWrapper, int count, ICacheLockWrapper handle)
        {
            cacheWrapper.PutAndUnlock(_typeCount, count, handle);
        }

        public int GetCount(ICacheWrapper cacheWrapper)
        {
            var result = TryGet(cacheWrapper, w => w.Get<int?>(_typeCount));
            return result ?? 0;
        }

        public void PutCount(ICacheWrapper cacheWrapper, int count)
        {
            TryPut(cacheWrapper, (w, c) => w.Put(_typeCount, c), count);
        }

        public async Task<T[]> GetAsync(ICacheWrapper cacheWrapper, int key)
        {
            try
            {
                return await cacheWrapper.GetAsync<T[]>(GetKeyString(key));
            }
            catch (CacheException ex)
            {
                OnException(ex);
                Logger.Warn(ex);
                return null;
            }
        }

        public async Task PutAsync(ICacheWrapper cacheWrapper, int key, T[] dtos)
        {
            try
            {
                await cacheWrapper.PutAsync(GetKeyString(key), dtos);
            }
            catch (CacheException ex)
            {
                OnException(ex);
                Logger.Warn(ex);
           }
        }
    }
}
