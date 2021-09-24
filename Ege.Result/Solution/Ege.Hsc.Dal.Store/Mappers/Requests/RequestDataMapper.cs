namespace Ege.Hsc.Dal.Store.Mappers.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Dal.Entities;

    class RequestDataMapper : DataReaderMapper<RequestDataPage>
    {
        private const string Id = "Id";
        private const string State = "State";
        private const string Date = "CreateDate";
        private const string ParticipantId = "ParticipantId";
        private const string BlankState = "BlankState";
        private const string RecordCount = "RecordCount";

        public async override Task<RequestDataPage> Map(DbDataReader @from)
        {
            var idOrdinal = GetOrdinal(from, Id);
            var stateOrdinal = GetOrdinal(from, State);
            var dateOrdinal = GetOrdinal(from, Date);
            var participantIdOrdinal = GetOrdinal(from, ParticipantId);
            var blankStateOrdinal = GetOrdinal(from, BlankState);
            var recordCountOrdinal = GetOrdinal(from, RecordCount);
            var noteOrdinal = GetOrdinal(from, "Note");

            if (!await from.ReadAsync())
            {
                return new RequestDataPage
                {
                    Count = 0,
                    Page = new List<RequestData>(),
                };
            }

            var result = new List<RequestData>();
            var page = new RequestDataPage
            {
                Count = from.GetInt32(recordCountOrdinal),
                Page = result,
            };
            Guid? lastId = null;
            int? lastParticipantId = null;
            RequestData current = null;
            RequestParticipantData currentParticipant = null;
            do
            {
                var currentId = from.GetGuid(idOrdinal);
                if (currentId != lastId)
                {
                    current = new RequestData
                    {
                        Id = currentId,
                        CreateDate = from.GetDateTime(dateOrdinal),
                        State = (BlankRequestState) from.GetInt32(stateOrdinal),
                        Participants = new List<RequestParticipantData>(),
                        Note = await from.GetNullableStringAsync(noteOrdinal),
                    };
                    result.Add(current);
                    lastId = currentId;
                    lastParticipantId = null;
                }
                var currentParticipantId = await from.GetNullableInt32Async(participantIdOrdinal);
                if (currentParticipantId.HasValue)
                {
                    if (currentParticipantId.Value != lastParticipantId)
                    {
                        currentParticipant = new RequestParticipantData
                        {
                            Id = currentParticipantId.Value,
                            State = Enum.GetValues(typeof(BlankDownloadState)).Cast<BlankDownloadState>()
                                .ToDictionary(s => s, s => 0),
                        };
                        current.Participants.Add(currentParticipant);
                    }
                    var state = (BlankDownloadState?) (await from.GetNullableInt32Async(blankStateOrdinal));
                    if (state.HasValue)
                    {
                        ++currentParticipant.State[state.Value];
                    }
                }
                else
                {
                    ++current.NotFoundParticipants;
                }
                lastParticipantId = currentParticipantId;
            } while (await from.ReadAsync());

            return page;
        }
    }
}
