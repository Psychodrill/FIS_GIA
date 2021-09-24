namespace Ege.Check.Logic.Services.Staff.Rcoi
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IRcoiInfoService
    {
        [NotNull]
        Task<RcoiInfo> GetRcoiInfoByRegion(int regionId);

        [NotNull]
        Task UpdateRcoiInfo(int regionId, [NotNull] RcoiInfo rcoi);
    }
}