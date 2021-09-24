namespace Ege.Hsc.Logic.Servers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Ege.Hsc.Logic.Models.Servers;
    using JetBrains.Annotations;

    public interface IServerService
    {
        [NotNull]
        Task CheckServersAvailability();

        [NotNull]
        Task LoadServersFromCheckEgeDb();

        [NotNull]
        Task<BlankServerStatus> CheckStatus(int regionId);

        [NotNull]
        Task<ICollection<BlankServerStatus>> CheckAllStatuses();

        [NotNull]
        Task<ICollection<BlankServerStatus>> GetStatuses(int? regionId = null);

        [NotNull]
        Task<Stream> GenerateErrorsFile(int regionId);
    }
}
