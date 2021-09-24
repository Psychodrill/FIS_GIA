namespace Ege.Check.Logic.Services.Participant.Regions
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IRegionService
    {
        [NotNull]
        ICollection<AvailableRegion> GetAvailableRegions();
    }
}