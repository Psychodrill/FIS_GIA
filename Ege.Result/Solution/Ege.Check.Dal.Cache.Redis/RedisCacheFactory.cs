namespace Ege.Check.Dal.Cache.Redis
{
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using global::Common.Logging;
    using JetBrains.Annotations;
    using StackExchange.Redis;

    public class RedisCacheFactory : ICacheFactory
    {
        private static ConnectionMultiplexer _multiplexer;

        [NotNull] private static readonly object LockObject = new object();

        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<RedisCacheFactory>();

        [NotNull] private readonly ICacheSettingsProvider _cacheSettingsProvider;
        [NotNull] private readonly ICacheFailureHelper _failureHelper;
        [NotNull] private readonly ICacheSerializer _cacheSerializer;
        private readonly int _cacheNumber;

        public RedisCacheFactory(
            [NotNull] ICacheSettingsProvider cacheSettingsProvider,
            [NotNull] ICacheFailureHelper failureHelper, 
            [NotNull] ICacheSerializer cacheSerializer)
        {
            _cacheSettingsProvider = cacheSettingsProvider;
            _failureHelper = failureHelper;
            _cacheSerializer = cacheSerializer;

            _cacheNumber = _cacheSettingsProvider.GetCacheNumber();
        }

        [NotNull]
        private ConnectionMultiplexer Multiplexer
        {
            get { return _multiplexer ?? LockGetMultiplexer(); }
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
                return new RedisCacheWrapper(Multiplexer.GetDatabase(_cacheNumber), _cacheSerializer);
            }
            catch (RedisException ex)
            {
                DisposeCacheObjects();
                _failureHelper.Failed();
                Logger.Warn(ex);
                return null;
            }
        }

        [NotNull]
        private ConnectionMultiplexer LockGetMultiplexer()
        {
            lock (LockObject)
            {
                return _multiplexer = ConnectionMultiplexer.Connect(_cacheSettingsProvider.GetCacheSettings());
            }
        }

        private static void DisposeCacheObjects()
        {
            _multiplexer = null;
        }
    }
}
