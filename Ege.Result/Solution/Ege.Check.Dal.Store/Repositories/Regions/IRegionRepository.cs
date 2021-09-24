namespace Ege.Check.Dal.Store.Repositories.Regions
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IRegionRepository
    {
        [NotNull]
        Task<ICollection<AvailableRegion>> GetAvailableRegions([NotNull] DbConnection connection);
    }
}