namespace Ege.Check.Dal.Cache.Captcha
{
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Logic.Models;
    using JetBrains.Annotations;

    public interface ICaptchaCache : ICache<int, CachedCaptcha>
    {
        void UnsafePut([NotNull] ICacheWrapper cache, int number, CachedCaptcha captcha);

        int? UnsafeGetCurrentCount([NotNull] ICacheWrapper cache);

        int? GetCurrentCount(ICacheWrapper cache);

        void SetCurrentCount(ICacheWrapper cache, int count);

        ICacheLockWrapper LockCurrentCount(ICacheWrapper cache);

        void UnlockCurrentCount(ICacheWrapper cache, [NotNull]ICacheLockWrapper handle, int count);
    }
}