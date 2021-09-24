namespace Ege.Check.Dal.MemoryCache.Regions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal class RegionSettingsMemoryCache : IRegionSettingsMemoryCache
    {
        [NotNull] private static volatile IDictionary<int, RegionSettingsCacheModel> _cache =
            new Dictionary<int, RegionSettingsCacheModel>();

        public RegionSettingsCacheModel Get(int regionId)
        {
            RegionSettingsCacheModel result;
            if (!_cache.TryGetValue(regionId, out result) || result == null)
            {
                result = new RegionSettingsCacheModel
                    {
                        Settings = new Dictionary<int, RegionExamSettingCacheModel>(),
                        Info = new RegionInfoCacheModel(),
                        Servers = new BlanksServerCacheModel(),
                    };
            }
            return result;
        }

        public void Put(IDictionary<int, RegionSettingsCacheModel> settings)
        {
            _cache = settings;
        }

        public IReadOnlyDictionary<int, RegionSettingsCacheModel> GetAll()
        {
            return new ReadOnlyDictionary<int, RegionSettingsCacheModel>(_cache);
        }
    }
}
