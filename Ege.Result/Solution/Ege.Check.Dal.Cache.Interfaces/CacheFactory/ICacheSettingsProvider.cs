namespace Ege.Check.Dal.Cache.Interfaces.CacheFactory
{
    using JetBrains.Annotations;

    public interface ICacheSettingsProvider
    {
        [NotNull]
        string GetCacheName();

        [NotNull]
        string GetCacheSettings();

        int GetCacheNumber();

        int GetCacheWaitLockQuantum();

        int GetCacheMaxLockDuration();
    }
}