namespace Ege.Check.Dal.Cache.Captcha
{
    using System;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Logic.Models;
    using JetBrains.Annotations;

    internal class CaptchaCache : BaseCache<CachedCaptcha, int>, ICaptchaCache
    {
        public CaptchaCache([NotNull] ICacheFailureHelper failureHelper)
            : base(failureHelper)
        {
        }

        public int? UnsafeGetCurrentCount(ICacheWrapper cache)
        {
            return UnsafeGetCurrentCountObject(cache);
        }

        public int? GetCurrentCount([NotNull] ICacheWrapper cache)
        {
            return TryGet(cache, UnsafeGetCurrentCountObject);
        }

        public void SetCurrentCount(ICacheWrapper cache, int count)
        {
            TryPut(cache, UnsafePutCurrentCount, count as object);
        }

        public ICacheLockWrapper LockCurrentCount([NotNull] ICacheWrapper cache)
        {
            ICacheLockWrapper result;
            try
            {
                cache.GetAndLock<int?>(Constants.CaptchaCountKey, TimeSpan.FromMinutes(1), out result);
            }
            catch (CacheException ex)
            {
                Logger.Warn(ex);
                return null;
            }
            return result;
        }

        public void UnlockCurrentCount([NotNull] ICacheWrapper cache, ICacheLockWrapper handle, int count)
        {
            try
            {
                cache.PutAndUnlock(Constants.CaptchaCountKey, count, handle);
            }
            catch (CacheException ex)
            {
                Logger.Warn(ex);
            }
        }

        public void UnsafePut(ICacheWrapper cache, int number, CachedCaptcha captcha)
        {
            cache.Put(GetKeyString(number), captcha);
        }

        private static int? UnsafeGetCurrentCountObject([NotNull] ICacheWrapper cache)
        {
            return cache.Get<int?>(Constants.CaptchaCountKey);
        }

        protected override string GetKeyString(int key)
        {
            return string.Format("Captcha{0}", key);
        }

        private static void UnsafePutCurrentCount([NotNull] ICacheWrapper cache, object count)
        {
            cache.Put(Constants.CaptchaCountKey, count);
        }
    }
}
