namespace Ege.Check.Dal.Cache.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;

    public interface ICache<in TKey, TCached>
        where TCached : class
    {
        TCached Get(ICacheWrapper wrapper, TKey key);

        void Put(ICacheWrapper wrapper, TKey key, TCached item);
    }

    public interface ILockableCache<TKey, TCached> : ICache<TKey, TCached> 
        where TCached : class
    {
        [NotNull]
        Task<CacheLock<TCached>> LockGet(ICacheWrapper wrapper, TKey key, bool createIfNotExists = true);

        [NotNull]
        Task<CacheBulkLock<TKey, TCached>> LockBulkGet([NotNull]ICacheWrapper cacheWrapper, [NotNull]IEnumerable<TKey> keys, bool createIfNotExists = true);
    }
}
