using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GetOlympicsDiplomants
    {
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
        public long OlympicDiplomantDocumentId { get; set; }
        public long Expr1 { get; set; }
        public DateTime? BirthDate { get; set; }
        public int IdentityDocumentTypeId { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public string OrganizationIssue { get; set; }
        public DateTime? DateIssue { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
    }
}
