namespace Ege.Check.Captcha
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.Captcha;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Logic.Models;
    using JetBrains.Annotations;
    using global::Common.Logging;

    internal class InMemoryDbCacheCaptchaService : ICaptchaService
    {
        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<InMemoryDbCacheCaptchaService>();

        private static int? _currentCount;
        [NotNull] private readonly ICaptchaCache _cache;
        [NotNull] private readonly ICacheFactory _factory;
        [NotNull] private readonly ICaptchaGenerator _generator;
        [NotNull] private readonly ICaptchaCacheSettingsProvider _settings;

        public InMemoryDbCacheCaptchaService(
            [NotNull] ICaptchaCache cache,
            [NotNull] ICacheFactory factory,
            [NotNull] ICaptchaGenerator generator,
            [NotNull] ICaptchaCacheSettingsProvider settings)
        {
            _cache = cache;
            _factory = factory;
            _generator = generator;
            _settings = settings;
        }

        public void TryGenerateBatch()
        {
            try
            {
                var cacheConnection = _factory.GetCache();
                if (cacheConnection == null)
                {
                    return;
                }
                try
                {
                    _currentCount = _cache.UnsafeGetCurrentCount(cacheConnection);
                }
                catch (CacheException ex)
                {
                    _currentCount = null;
                    Logger.Warn(ex);
                }
                var maxCount = _settings.GetMaxCacheSize();
                if (_currentCount >= maxCount)
                {
                    return;
                }
                if (_currentCount == null)
                {
                    _cache.SetCurrentCount(cacheConnection, 0);
                }
                _currentCount = _currentCount ?? 0;
                var handle = _cache.LockCurrentCount(cacheConnection);
                if (handle == null)
                {
                    return;
                }
                var batchSize = Math.Min(_settings.GetBatchSize(), maxCount - _currentCount.Value);
                foreach (var i in Enumerable.Range(_currentCount.Value, batchSize))
                {
                    var captcha = _generator.GenerateOne();
                    _cache.UnsafePut(cacheConnection, i, captcha);
                }
                _cache.UnlockCurrentCount(cacheConnection, handle, _currentCount.Value + batchSize);
            }
            catch (CacheException ex)
            {
                Logger.Warn(ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public int CurrentCachedCount()
        {
            return _currentCount ?? 0;
        }

        public Task<CachedCaptcha> Retrieve(int id)
        {
            var cacheConnection = _factory.GetCache();
            var result = _cache.Get(cacheConnection, id);
            if (result == null)
            {
                Logger.Trace("Cache returned null, generating a new captcha");
                result = _generator.GenerateOne();
                _cache.Put(cacheConnection, id, result);
            }
            return Task.FromResult(result);
        }

        public bool Generatable
        {
            get { return true; }
        }
    }
}