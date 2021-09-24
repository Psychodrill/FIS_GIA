namespace Ege.Hsc.Dal.Store.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Models;
    using Ege.Hsc.Logic.Models.Requests;
    using JetBrains.Annotations;

    public interface IBlankRequestRepository
    {
        /// <summary>
        /// Получить не более maxCount запросов на выгрузку, все бланки в которых обработаны (т.е. загружены успешно или не загружены из-за ошибки)
        /// </summary>
        [NotNull]
        Task<ICollection<BlankRequest>> TopNotZippedAsync([NotNull] DbConnection connection, int maxCount);

        /// <summary>
        /// Установить статус запроса в "Архив сформирован"
        /// </summary>
        [NotNull]
        Task SetZipped([NotNull]DbConnection connection, Guid requestId);

        [NotNull]
        Task<RequestedParticipant> GetRequestedParticipant([NotNull]DbConnection connection, string hash, string document);

        [NotNull]
        Task<Guid> AddSingleParticipantRequest([NotNull] DbConnection connection, [NotNull]UserReference user, int participantId);

        [NotNull]
        Task<RequestDataPage> GetRequestsData([NotNull] DbConnection connection, [NotNull]UserReference user, int skip, int take);

        [NotNull]
        Task<Guid> CreateRequest([NotNull] DbConnection connection, [NotNull]UserReference user, string note, [NotNull]IEnumerable<RequestedParticipant> participants);

        [NotNull]
        Task<RequestPermission> IsRequestOwner([NotNull] DbConnection connection, Guid requestId, [NotNull]UserReference user);

        [NotNull]
        Task<OldRequests> GetOldRequestIds([NotNull] DbConnection connection, int requestResultHoursToLive);

        [NotNull]
        Task<int> SetDeletedStatusForOldRequests([NotNull] DbConnection connection, [NotNull] OldRequests oldRequests);
    }
}
