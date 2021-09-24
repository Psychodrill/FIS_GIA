namespace Ege.Check.Logic.Services.Participant.Regions
{
    using System.Collections.Generic;
    using Ege.Check.Dal.MemoryCache.Regions;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal class RegionService : IRegionService
    {
        [NotNull] private readonly IRegionMemoryCache _cache;

        public RegionService([NotNull] IRegionMemoryCache cache)
        {
            _cache = cache;
        }

        public ICollection<AvailableRegion> GetAvailableRegions()
        {
            var result = _cache.Get();
            return result;
        }
    }
}