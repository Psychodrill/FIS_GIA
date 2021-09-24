namespace Ege.Check.Dal.Cache.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using global::Common.Logging;
    using JetBrains.Annotations;
    using StackExchange.Redis;

    internal class RedisCacheWrapper : CacheWrapperBase<RedisException>, ICacheWrapper
    {
        [NotNull] private readonly IDatabase _cache;
        [NotNull] private readonly ICacheSerializer _serializer;
        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<RedisCacheWrapper>();

        public RedisCacheWrapper([NotNull] IDatabase cache, [NotNull] ICacheSerializer serializer)
        {
            _cache = cache;
            _serializer = serializer;
        }

        internal IDatabase CacheObject
        {
            get { return _cache; }
        }

        public T Get<T>(string key)
        {
            return ExecuteAndWrapException(() => _serializer.Deserialize<T>(_cache.StringGet(key)));
        }

        public void Put(string key, object item)
        {
            ExecuteAndWrapException(() =>
                {
                    if (item == null)
                    {
                        _cache.KeyDelete(key);
                    }
                    else
                    {
                        _cache.StringSet(key, _serializer.Serialize(item));
                    }
                });
        }

        public T GetAndLock<T>(string key, TimeSpan lockDuration, out ICacheLockWrapper handle)
        {
            RedisLockWrapper lockHandle = null;
            var cachedObject = ExecuteAndWrapException(() =>
            {
                lockHandle = new RedisLockWrapper { LockId = Guid.NewGuid().ToString() };
                if (!_cache.LockTake(KeyOfLock(key), lockHandle.LockId, lockDuration))
                {
                    throw new LivingCacheException(string.Format("Lock on {0} already taken", key), null);
                }
                var result = _cache.StringGet(key);
                return result;
            });
            handle = lockHandle;
            return _serializer.Deserialize<T>(cachedObject);
        }

        public void PutAndUnlock(string key, object item, ICacheLockWrapper handle)
        {
            ExecuteAndWrapException(() =>
                {
                    var redisHandle = (RedisLockWrapper) handle;
                    if (item == null)
                    {
                        _cache.KeyDelete(key);
                    }
                    else
                    {
                        _cache.StringSet(key, _serializer.Serialize(item));
                    }
                    _cache.LockRelease(KeyOfLock(key), redisHandle.LockId);
                });
        }

        public override bool IsLivingCacheException(RedisException exception)
        {
            return false;
        }

        private string KeyOfLock(string keyOfLocked)
        {
            return string.Format("{0}:lock", keyOfLocked);
        }

        public bool SupportsBulkOperations
        {
            get { return true; }
        }

        public Task<IEnumerable<T>> BulkGet<T>(IEnumerable<string> keys)
        {
            return ExecuteAndWrapExceptionAsync(async () =>
            {
                var values = (await _cache.StringGetAsync(keys.ToRedisKeys())).Select(v => _serializer.Deserialize<T>(v));
                return values;
            });
        }

        public Task BulkPut<T>(IEnumerable<KeyValuePair<string, T>> keyValuePairs)
        {
            return ExecuteAndWrapExceptionAsync(async () =>
            {
                await _cache.StringSetAsync(keyValuePairs.ToRedisKeyValuePairs(_serializer));
            });
        }


        public Task<T> GetAsync<T>(string key)
        {
            return ExecuteAndWrapExceptionAsync(async () => _serializer.Deserialize<T>(await _cache.StringGetAsync(key)));
        }

        public Task PutAsync(string key, object item)
        {
            return ExecuteAndWrapExceptionAsync(async () =>
                {
                    await _cache.StringSetAsync(key, _serializer.Serialize(item));
                });
        }

        public void ExtendLock(string lockKey, int maxLockDuration, ICacheLockWrapper lockWrapper)
        {
            ExecuteAndWrapException(() => _cache.LockExtend(lockKey, ((RedisLockWrapper)lockWrapper).LockId, TimeSpan.FromMilliseconds(maxLockDuration)));
        }

        public async Task Clear()
        {
            var endPoints = _cache.Multiplexer.GetEndPoints();
            foreach (var endPoint in endPoints)
            {
                var server = _cache.Multiplexer.GetServer(endPoint);
                try
                {
                    await server.FlushAllDatabasesAsync();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
    }
}
