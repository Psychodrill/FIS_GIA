namespace Ege.Check.Dal.Cache.LoadServices.DtoCache
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;

    public interface IDtoCache<T>
    {
        [NotNull]
        Task<T[]> GetAsync([NotNull] ICacheWrapper cacheWrapper, int key);

        [NotNull]
        Task PutAsync([NotNull] ICacheWrapper cacheWrapper, int key, T[] dtos);

        int GetCount([NotNull] ICacheWrapper cacheWrapper);

        void PutCount([NotNull] ICacheWrapper cacheWrapper, int count);

        Task<KeyValuePair<int, ICacheLockWrapper>> LockGetCount([NotNull]ICacheWrapper cacheWrapper);

        void UnlockPutCount([NotNull]ICacheWrapper cacheWrapper, int count, ICacheLockWrapper handle);
    }
}
