namespace Ege.Hsc.Dal.Entities
{
    using System;

    public class RequestedParticipant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsCollision { get; set; }


        public bool HasErrors { get; set; }

        
        public Guid? RbdId { get; set; }

        public string Hash { get; set; }

        public string DocumentNumber { get; set; }

        public int? RegionId { get; set; }

        public string Region { get; set; }

        
        public bool ProcessingUnfinished { get; set; }

        public bool HasNoServerUrl { get; set; }

        public bool HasNoBlanks { get; set; }
    }
}
