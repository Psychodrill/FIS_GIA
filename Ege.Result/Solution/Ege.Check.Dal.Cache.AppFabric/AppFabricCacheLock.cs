namespace Ege.Check.Dal.Cache.AppFabric
{
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Microsoft.ApplicationServer.Caching;

    public class DataCacheLockWrapper : ICacheLockWrapper
    {
        public DataCacheLockHandle Handle { get; set; }
    }
}
