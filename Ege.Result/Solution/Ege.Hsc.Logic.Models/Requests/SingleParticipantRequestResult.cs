namespace Ege.Hsc.Logic.Models.Requests
{
    using System;

    public class SingleParticipantRequestResult
    {
        public SingleParticipantRequestStatus Status { get; set; }

        public Guid? Id { get; set; }
    }
}
