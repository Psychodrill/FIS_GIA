namespace Ege.Check.Dal.MemoryCache.Regions
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IRegionSettingsMemoryCache
    {
        [NotNull]
        RegionSettingsCacheModel Get(int regionId);

        [NotNull]
        IReadOnlyDictionary<int, RegionSettingsCacheModel> GetAll(); 

        void Put([NotNull] IDictionary<int, RegionSettingsCacheModel> settings);
    }
}