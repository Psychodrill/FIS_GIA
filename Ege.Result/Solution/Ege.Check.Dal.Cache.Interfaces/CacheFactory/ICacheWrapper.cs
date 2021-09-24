namespace Ege.Check.Dal.Cache.Interfaces.CacheFactory
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface ICacheWrapper
    {
        T Get<T>(string key);
        void Put(string key, object item);
        T GetAndLock<T>(string key, TimeSpan lockDuration, out ICacheLockWrapper handle);
        void PutAndUnlock(string key, object item, [NotNull]ICacheLockWrapper handle);

        void ExtendLock(string lockKey, int maxLockDuration, [NotNull]ICacheLockWrapper lockWrapper);

        [NotNull]
        Task<T> GetAsync<T>(string key);
        [NotNull]
        Task PutAsync(string key, object item);

        [NotNull]
        Task<IEnumerable<T>> BulkGet<T>(IEnumerable<string> keys);
        [NotNull]
        Task BulkPut<T>(IEnumerable<KeyValuePair<string, T>> keyValuePairs);

        bool SupportsBulkOperations { get; }

        Task Clear();
    }
}
