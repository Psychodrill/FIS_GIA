namespace Ege.Check.Dal.Cache
{
    using System;
    using System.Runtime.Serialization;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using global::Common.Logging;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    /// <summary>
    ///     Обёртка над кэшем для ловли и глотания его исключений
    /// </summary>
    public abstract class SafeCache
    {
        [NotNull] protected readonly ILog Logger;

        [NotNull] private readonly ICacheFailureHelper _cacheFailureHelper;

        protected SafeCache([NotNull] ICacheFailureHelper cacheFailureHelper)
        {
            _cacheFailureHelper = cacheFailureHelper;
            Logger = LogManager.GetLogger(GetType());
        }

        protected void OnException([NotNull] CacheException exception)
        {
            _cacheFailureHelper.Failed();
            Logger.Warn(exception);
        }

        protected void OnDeserializationException([NotNull] Exception exception)
        {
            Logger.Warn(exception);
        }

        protected T TryGet<T>(ICacheWrapper cache, [NotNull] Func<ICacheWrapper, T> getFunc)
        {
            if (cache == null || _cacheFailureHelper.IsCacheFailed() || _cacheFailureHelper.IsGetProhibited())
            {
                return default(T);
            }
            try
            {
                return getFunc(cache);
            }
            catch (CacheException ex)
            {
                OnException(ex);
                return default(T);
            }
            catch (InvalidDataContractException ex)
            {
                OnDeserializationException(ex);
                return default(T);
            }
            catch (JsonSerializationException ex)
            {
                OnDeserializationException(ex);
                return default(T);
            }
        }

        protected T TryGet<T, TParam>(ICacheWrapper cache, [NotNull] Func<ICacheWrapper, TParam, T> getFunc,
                                      TParam param)
        {
            if (cache == null || _cacheFailureHelper.IsCacheFailed() || _cacheFailureHelper.IsGetProhibited())
            {
                return default(T);
            }
            try
            {
                return getFunc(cache, param);
            }
            catch (CacheException ex)
            {
                OnException(ex);
                return default(T);
            }
            catch (InvalidDataContractException ex)
            {
                OnDeserializationException(ex);
                return default(T);
            }
            catch (JsonSerializationException ex)
            {
                OnDeserializationException(ex);
                return default(T);
            }
        }

        protected void TryPut<TObj>(ICacheWrapper cache, [NotNull] Action<ICacheWrapper, TObj> putAction, TObj obj)
        {
            if (cache == null || _cacheFailureHelper.IsCacheFailed())
            {
                return;
            }
            try
            {
                putAction(cache, obj);
            }
            catch (CacheException ex)
            {
                OnException(ex);
            }
        }

        protected void TryPut<TObj>(ICacheWrapper cache, [NotNull] Action<ICacheWrapper, string, TObj> putAction,
                                    string key, TObj obj)
        {
            if (cache == null || _cacheFailureHelper.IsCacheFailed())
            {
                return;
            }
            try
            {
                putAction(cache, key, obj);
            }
            catch (CacheException ex)
            {
                OnException(ex);
            }
        }
    }
}