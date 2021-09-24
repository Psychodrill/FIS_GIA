namespace Ege.Check.Dal.Cache
{
    using System.Configuration;
    using Ege.Check.Common.Config;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;

    internal class CacheSettingsProvider : ICacheSettingsProvider
    {
        [NotNull] private readonly IConfigReaderHelper _reader;

        public CacheSettingsProvider([NotNull] IConfigReaderHelper reader)
        {
            _reader = reader;
        }

        public string GetCacheName()
        {
            var result = ConfigurationManager.AppSettings["CacheInstanceName"];
            if (result == null)
            {
                throw new ConfigurationErrorsException(
                    "В web.config не указано имя экземпляра кэша (appSettings/CacheInstanceName)");
            }
            return result;
        }


        public int GetCacheWaitLockQuantum()
        {
            return _reader.GetInt("CacheWaitForLockQuantumMilliseconds", "время между попытками взятия лока в AppFabric", 100);
        }

        public int GetCacheMaxLockDuration()
        {
            return _reader.GetInt("CacheMaximumLockDurationMilliseconds", "максимальная длительность лока в AppFabric", 15000);
        }


        public string GetCacheSettings()
        {
            return _reader.GetString("CacheHost", "url кэша (appSettings/CacheHost)");
        }


        public int GetCacheNumber()
        {
            return _reader.GetInt("CacheNumber", "идентификатор кэша", 0);
        }
    }
}
