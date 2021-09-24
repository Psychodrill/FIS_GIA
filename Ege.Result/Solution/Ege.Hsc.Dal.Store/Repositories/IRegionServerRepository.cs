namespace Ege.Hsc.Dal.Store.Repositories
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Hsc.Logic.Models.Servers;
    using JetBrains.Annotations;

    public interface IRegionServerRepository
    {
        [NotNull]
        Task LoadFromCheckEgeDb([NotNull]DbConnection connection);

        [NotNull]
        Task<ICollection<BlankServerAvailabilityModel>> GetServersHavingBlanks([NotNull] DbConnection connection);

        [NotNull]
        Task<ICollection<ServerBlanks>> GetServersWithBlanks([NotNull] DbConnection connection, int? regionId);

        [NotNull]
        Task UpdateServerAvailability([NotNull] DbConnection connection, [NotNull] IDictionary<int, bool> availabilityByRegion);

        [NotNull]
        Task UpdateServerData([NotNull] DbConnection connection, int regionId, int serverBlankCount, [NotNull] IEnumerable<ServerErrors> errors, bool isAvailable);

        [NotNull]
        Task<ICollection<BlankServerStatus>> GetStatuses([NotNull] DbConnection connection, int? regionId = null);

        [NotNull]
        Task<ICollection<BlankServerError>> GetErrors([NotNull] DbConnection connection, int regionId);
    }
}
