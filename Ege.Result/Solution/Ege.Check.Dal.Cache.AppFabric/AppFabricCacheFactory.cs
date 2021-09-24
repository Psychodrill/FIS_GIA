namespace Ege.Check.Dal.Cache.AppFabric
{
    using System;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using global::Common.Logging;
    using JetBrains.Annotations;
    using Microsoft.ApplicationServer.Caching;

    public class AppFabricCacheFactory : ICacheFactory
    {
        private static DataCacheFactory _factory;

        [ThreadStatic] private static ICacheWrapper _cache;

        [NotNull] private static readonly object LockObject = new object();

        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<AppFabricCacheFactory>();

        [NotNull] private readonly ICacheSettingsProvider _cacheSettingsProvider;

        [NotNull] private readonly ICacheFailureHelper _failureHelper;

        public AppFabricCacheFactory([NotNull] ICacheSettingsProvider cacheSettingsProvider,
                            [NotNull] ICacheFailureHelper failureHelper)
        {
            _cacheSettingsProvider = cacheSettingsProvider;
            _failureHelper = failureHelper;
        }

        [NotNull]
        private DataCacheFactory Factory
        {
            get { return _factory ?? LockGetFactory(); }
        }

        public ICacheWrapper GetCache()
        {
            if (_failureHelper.IsCacheFailed())
            {
                DisposeCacheObjects();
                return null;
            }
            try
            {
                return _cache ?? (_cache = LockGetCache());
            }
            catch (DataCacheException ex)
            {
                DisposeCacheObjects();
                _failureHelper.Failed();
                Logger.Warn(ex);
                return null;
            }
        }

        [NotNull]
        private DataCacheFactory LockGetFactory()
        {
            lock (LockObject)
            {
                return _factory = new DataCacheFactory(new DataCacheFactoryConfiguration());
            }
        }

        [NotNull]
        private ICacheWrapper LockGetCache()
        {
            lock (LockObject)
            {
                return _cache = new AppFabricCacheWrapper(Factory.GetCache(_cacheSettingsProvider.GetCacheName()));
            }
        }

        private static void DisposeCacheObjects()
        {
            _factory = null;
            _cache = null;
        }
    }
}