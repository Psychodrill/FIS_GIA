namespace Ege.Check.Dal.MemoryCache.Regions
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public class RegionMemoryCache : IRegionMemoryCache
    {
        [NotNull] private static volatile ICollection<AvailableRegion> _regions = new AvailableRegion[0];

        public ICollection<AvailableRegion> Get()
        {
            return _regions;
        }

        public void Put(ICollection<AvailableRegion> regions)
        {
            _regions = regions;
        }
    }
}