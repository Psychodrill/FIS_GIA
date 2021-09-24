namespace Ege.Check.Dal.Cache.Redis
{
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;

    public class RedisLockWrapper : ICacheLockWrapper
    {
        public string LockId { get; set; }
    }
}
