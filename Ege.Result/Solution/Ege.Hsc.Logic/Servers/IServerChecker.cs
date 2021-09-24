namespace Ege.Hsc.Logic.Servers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Servers;
    using Ege.Hsc.Logic.Models.Servers;
    using JetBrains.Annotations;

    public interface IServerChecker
    {
        [NotNull]
        Task<KeyValuePair<int, bool>> CheckAvailability([NotNull]BlankServerAvailabilityModel server);

        [NotNull]
        Task<ServerErrors> CheckFile(string serverUrl, KeyValuePair<ExamFolder, ISet<string>>  serverBlanks);
    }
}
