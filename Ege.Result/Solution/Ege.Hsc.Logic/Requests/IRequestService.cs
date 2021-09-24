namespace Ege.Hsc.Logic.Requests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Requests;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Models;
    using Ege.Hsc.Logic.Models.Requests;
    using JetBrains.Annotations;

    public interface IRequestService
    {
        [NotNull]
        Task<SingleParticipantRequestResult> ProcessSingleParticipant([NotNull]ParticipantBlankRequest request, [NotNull]UserReference user);

        [NotNull]
        Task<RequestStatusPage> GetRequests([NotNull]UserReference user, int skip, int take);

        [NotNull]
        Task<Guid> CreateRequest([NotNull]UserReference user, string note, [NotNull]IEnumerable<Task<Stream>> csv);

        [NotNull]
        Task<RequestPermission> IsRequestOwner(Guid requestId, [NotNull]UserReference user);

        [NotNull]
        Task<int> DeleteZipsForOldRequests();
    }
}
