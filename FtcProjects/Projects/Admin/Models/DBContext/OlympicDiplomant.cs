using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicDiplomant
    {
        public OlympicDiplomant()
        {
            OlympicDiplomantDocument = new HashSet<OlympicDiplomantDocument>();
        }

        public long OlympicDiplomantId { get; set; }
        public int OlympicTypeProfileId { get; set; }
        public long? OlympicDiplomantIdentityDocumentId { get; set; }
        public int? SchoolRegionId { get; set; }
        public long? SchoolEgeCode { get; set; }
        public string SchoolEgeName { get; set; }
        public int? FormNumber { get; set; }
        public string DiplomaSeries { get; set; }
        public string DiplomaNumber { get; set; }
        public DateTime? DiplomaDateIssue { get; set; }
        public short ResultLevelId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string Comment { get; set; }
        public int? StatusId { get; set; }
        public string AdoptionUnfoundedComment { get; set; }
        public int? PersonId { get; set; }
        public DateTime? PersonLinkDate { get; set; }
        public int? EndingDate { get; set; }

        public virtual OlympicDiplomantDocument OlympicDiplomantIdentityDocument { get; set; }
        public virtual OlympicTypeProfile OlympicTypeProfile { get; set; }
        public virtual Rvipersons Person { get; set; }
        public virtual OlympicDiplomType ResultLevel { get; set; }
        public virtual RegionType SchoolRegion { get; set; }
        public virtual OlympicDiplomantStatus Status { get; set; }
        public virtual ICollection<OlympicDiplomantDocument> OlympicDiplomantDocument { get; set; }
    }
}
