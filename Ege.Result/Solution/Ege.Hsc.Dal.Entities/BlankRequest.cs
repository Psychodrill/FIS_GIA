namespace Ege.Hsc.Dal.Entities
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class BlankRequest
    {
        public Guid Id { get; set; }

        [NotNull]
        public ICollection<RequestedParticipant> Participants { get; set; }
    }
}