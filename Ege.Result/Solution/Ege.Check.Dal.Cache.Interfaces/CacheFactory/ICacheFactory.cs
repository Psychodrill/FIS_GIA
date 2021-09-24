namespace Ege.Check.Dal.Cache.CacheFactory
{
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;

    public interface ICacheFactory
    {
        ICacheWrapper GetCache();
    }
}