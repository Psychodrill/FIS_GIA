namespace Ege.Check.Dal.Cache
{
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;

    public abstract class BaseCache<TCached, TKey> : SafeCache, ICache<TKey, TCached>
        where TCached : class
    {
        protected BaseCache([NotNull] ICacheFailureHelper failureHelper)
            : base(failureHelper)
        {
        }

        private static TCached UnsafeGet([NotNull] ICacheWrapper cache, string key)
        {
            return cache.Get<TCached>(key);
        }

        private static void UnsafePut([NotNull] ICacheWrapper cache, string key, TCached value)
        {
            cache.Put(key, value);
        }

        public TCached Get(ICacheWrapper cache, TKey key)
        {
            return TryGet(cache, UnsafeGet, GetKeyString(key));
        }

        public void Put(ICacheWrapper cache, TKey key, TCached value)
        {
            TryPut(cache, UnsafePut, GetKeyString(key), value);
        }

        [NotNull]
        protected abstract string GetKeyString(TKey key);
    }
}
