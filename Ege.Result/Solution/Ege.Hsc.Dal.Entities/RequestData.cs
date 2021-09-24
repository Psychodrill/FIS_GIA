namespace Ege.Hsc.Dal.Entities
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class RequestDataPage
    {
        public int Count { get; set; }

        public ICollection<RequestData> Page { get; set; }
    }

    public class RequestData
    {
        public Guid Id { get; set; }

        public BlankRequestState State { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string Note { get; set; }

        public int NotFoundParticipants { get; set; }

        [NotNull]
        public ICollection<RequestParticipantData> Participants { get; set; }
    }

    public class RequestParticipantData
    {
        public int Id { get; set; }

        [NotNull]
        public IDictionary<BlankDownloadState, int> State { get; set; }
    }
}
