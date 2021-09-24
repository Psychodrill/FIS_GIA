namespace Ege.Hsc.Dal.Store.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Models;
    using Ege.Hsc.Logic.Models.Requests;
    using JetBrains.Annotations;

    internal class BlankRequestRepository : Repository, IBlankRequestRepository
    {
        private const string TopNotZippedProc = "GetTopBlankRequestsToZip";
        private const string SetRequestStateAndDeleteParticipantsProc = "SetRequestStateAndDeleteParticipants";
        private const string SetRequestStateAndDeleteSingleParticipantRequestsProc = "SetRequestStateAndDeleteSingleParticipantRequests";
        private const string GetParticipantProc = "GetParticipant";
        private const string CreateSingleParticipantRequestProc = "CreateSingleParticipantRequest";
        private const string GetRequestStateProc = "GetRequestState";
        private const string CreateRequestProc = "CreateRequest";
        private const string IsRequestOwnerProc = "IsRequestOwner";
        private const string GetOldRequestsProc = "GetOldRequests";
        private const string TopCountParam = "Top";
        private const string DownloadSuccessParam = "DownloadSuccessState";
        private const string DownloadErrorParam = "DownloadErrorState";
        private const string RequestOldStateParam = "RequestState";
        private const string RequestNewStateParam = "RequestNewState";
        private const string IdParam = "RequestId";
        private const string StateParam = "State";
        private const string HashParam = "Hash";
        private const string DocumentNumberParam = "DocumentNumber";
        private const string UserIdParam = "UserId";
        private const string ParticipantsParam = "Participants";
        private const string EsrpLoginParam = "EsrpUserLogin";
        private const string SkipParam = "Skip";
        private const string TakeParam = "Take";
        private const string NoteParam = "Note";
        private const string HoursToLiveParam = "HoursToLive";

        [NotNull] private readonly IDataReaderCollectionMapper<BlankRequest> _requestMapper;
        [NotNull] private readonly IDataReaderMapper<RequestedParticipant> _participantMapper;
        [NotNull] private readonly IDataReaderMapper<RequestDataPage> _requestStatusMapper;
        [NotNull] private readonly IDataTableMapper<IEnumerable<RequestedParticipant>> _participantTableMapper;
        [NotNull] private readonly IDataReaderMapper<RequestPermission> _requestPermissionMapper;

        public BlankRequestRepository(
            [NotNull] IDataReaderCollectionMapper<BlankRequest> requestMapper,
            [NotNull] IDataReaderMapper<RequestedParticipant> participantMapper,
            [NotNull] IDataReaderMapper<RequestDataPage> requestStatusMapper,
            [NotNull] IDataTableMapper<IEnumerable<RequestedParticipant>> participantTableMapper, 
            [NotNull] IDataReaderMapper<RequestPermission> requestPermissionMapper)
        {
            _requestMapper = requestMapper;
            _participantMapper = participantMapper;
            _requestStatusMapper = requestStatusMapper;
            _participantTableMapper = participantTableMapper;
            _requestPermissionMapper = requestPermissionMapper;
        }

        public async Task<ICollection<BlankRequest>> TopNotZippedAsync(DbConnection connection, int maxCount)
        {
            var cmd = StoredProcedureCommand(connection, TopNotZippedProc);
            AddParameter(cmd, TopCountParam, maxCount);
            AddParameter(cmd, DownloadSuccessParam, (int) BlankDownloadState.Downloaded);
            AddParameter(cmd, DownloadErrorParam, (int) BlankDownloadState.Error);
            AddParameter(cmd, RequestOldStateParam, (int) BlankRequestState.Queued);
            AddParameter(cmd, RequestNewStateParam, (int) BlankRequestState.Zipping);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await _requestMapper.Map(reader);
            }
        }

        public Task SetZipped(DbConnection connection, Guid requestId)
        {
            return SetState(connection, requestId, BlankRequestState.Zipped);
        }

        private async Task<int> SetState([NotNull] DbConnection connection, Guid requestId, BlankRequestState state)
        {
            var cmd = StoredProcedureCommand(connection, SetRequestStateAndDeleteParticipantsProc);
            AddParameter(cmd, IdParam, requestId);
            AddParameter(cmd, StateParam, (int) state);
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<RequestedParticipant> GetRequestedParticipant(DbConnection connection, string hash,
            string document)
        {
            var cmd = StoredProcedureCommand(connection, GetParticipantProc);
            AddParameter(cmd, HashParam, hash);
            AddParameter(cmd, DocumentNumberParam, document);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await _participantMapper.Map(reader);
            }
        }

        public async Task<Guid> AddSingleParticipantRequest(DbConnection connection, UserReference user, int participantId)
        {
            var result = Guid.NewGuid();
            var cmd = StoredProcedureCommand(connection, CreateSingleParticipantRequestProc);
            AddParameter(cmd, IdParam, result);
            AddParameter(cmd, UserIdParam, user.UserId);
            AddParameter(cmd, EsrpLoginParam, user.EsrpLogin);
            AddParameter(cmd, "ParticipantId", participantId);
            await cmd.ExecuteNonQueryAsync();
            return result;
        }
        
        public async Task<RequestDataPage> GetRequestsData(DbConnection connection, UserReference user, int skip, int take)
        {
            var cmd = StoredProcedureCommand(connection, GetRequestStateProc);
            AddParameter(cmd, UserIdParam, user.UserId);
            AddParameter(cmd, EsrpLoginParam, user.EsrpLogin);
            AddParameter(cmd, TakeParam, take);
            AddParameter(cmd, SkipParam, skip);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await _requestStatusMapper.Map(reader);
            }
        }

        public async Task<Guid> CreateRequest(DbConnection connection, UserReference user, string note, IEnumerable<RequestedParticipant> participants)
        {
            var table = _participantTableMapper.Map(participants);
            if (table == null || table.Rows == null || table.Rows.Count == 0)
            {
                throw new ArgumentException("Нет участников");
            }
            var result = Guid.NewGuid();
            var cmd = StoredProcedureCommand(connection, CreateRequestProc);
            AddParameter(cmd, IdParam, result);
            AddParameter(cmd, UserIdParam, user.UserId);
            AddParameter(cmd, EsrpLoginParam, user.EsrpLogin);
            AddParameter(cmd, StateParam, (int)BlankRequestState.Queued);
            AddParameter(cmd, ParticipantsParam, table);
            AddParameter(cmd, NoteParam, note);
            await cmd.ExecuteNonQueryAsync();
            return result;
        }

        public async Task<RequestPermission> IsRequestOwner(DbConnection connection, Guid requestId, UserReference user)
        {
            var cmd = StoredProcedureCommand(connection, IsRequestOwnerProc);
            AddParameter(cmd, IdParam, requestId);
            AddParameter(cmd, UserIdParam, user.UserId);
            AddParameter(cmd, EsrpLoginParam, user.EsrpLogin);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await _requestPermissionMapper.Map(reader);
            }
        }

        public async Task<OldRequests> GetOldRequestIds(DbConnection connection, int requestResultHoursToLive)
        {
            var cmd = StoredProcedureCommand(connection, GetOldRequestsProc);
            AddParameter(cmd, HoursToLiveParam, requestResultHoursToLive);
            AddParameter(cmd, StateParam, (int) BlankRequestState.Zipped);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                var result = new List<Guid>();
                while (await reader.ReadAsync())
                {
                    result.Add(reader.GetGuid(0));
                }
                return new OldRequests {Ids = result};
            }
        }

        public async Task<int> SetDeletedStatusForOldRequests(DbConnection connection, OldRequests oldRequests)
        {
            var cmd = StoredProcedureCommand(connection, SetRequestStateAndDeleteSingleParticipantRequestsProc);
            AddParameter(cmd, StateParam, (int) BlankRequestState.ZipDeleted);
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            foreach (var id in oldRequests.Ids)
            {
                table.Rows.Add(id);
            }
            AddParameter(cmd, IdParam, table);
            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
