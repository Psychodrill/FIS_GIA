namespace Ege.Hsc.Logic.Mappers
{
    using System;
    using System.Linq;
    using Ege.Check.Common;
    using Ege.Hsc.Dal.Entities;
    using TsSoft.Commons.Collections;

    class RequestDataToRequestStatusMapper : IMapper<RequestData, RequestStatus>
    {
        public RequestStatus Map(RequestData @from)
        {
            if (from == null || from.Participants == null)
            {
                throw new ArgumentNullException("from");
            }
            return new RequestStatus
            {
                Id = from.Id,
                CreateDate = from.CreateDate,
                TotalParticipants = from.Participants.Count + from.NotFoundParticipants,
                SuccessfullyProcessedParticipants = @from.Participants.Count(p => 
                    p.State.All(s => s.Key == BlankDownloadState.Downloaded || s.Value == 0) &&
                    p.State[BlankDownloadState.Downloaded] > 0),
                NotFoundParticipants = from.NotFoundParticipants,
                ProcessedWithErrorsParticipants = from.Participants.Count(p => p.State.All(
                    s => s.Key == BlankDownloadState.Downloaded || s.Key == BlankDownloadState.Error || s.Value == 0) && 
                    p.State[BlankDownloadState.Error] > 0),
                Total = from.Participants.Sum(p => p.State.Sum(s => s.Value)),
                Downloaded = from.Participants.Sum(p => p.State[BlankDownloadState.Downloaded]),
                Error = from.Participants.Sum(p => p.State[BlankDownloadState.Error]),
                State = Enums.GetEnumDescription(from.State),
                IsReady = from.State == BlankRequestState.Zipped,
                Note = from.Note,
            };
        }
    }
}
