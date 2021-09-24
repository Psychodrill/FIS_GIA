namespace Ege.Check.Dal.Store.Repositories.Regions
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IRcoiInfoRepository
    {
        [NotNull]
        Task<RcoiInfo> GetById(DbConnection connection, int regionId);

        [NotNull]
        Task UpdateRcoiInfo(DbConnection connection, int regionId, [NotNull] RcoiInfo rcoi);
    }
}