namespace Ege.Check.Dal.MemoryCache.Regions
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IRegionMemoryCache
    {
        [NotNull]
        ICollection<AvailableRegion> Get();

        void Put([NotNull] ICollection<AvailableRegion> regions);
    }
}