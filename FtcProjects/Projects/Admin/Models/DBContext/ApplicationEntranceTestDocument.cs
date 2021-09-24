using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationEntranceTestDocument
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int? SubjectId { get; set; }
        public int? EntrantDocumentId { get; set; }
        public short? EntranceTestTypeId { get; set; }
        public int? SourceId { get; set; }
        public decimal? ResultValue { get; set; }
        public short? BenefitId { get; set; }
        public int? EntranceTestItemId { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string InstitutionDocumentNumber { get; set; }
        public DateTime? InstitutionDocumentDate { get; set; }
        public int? InstitutionDocumentTypeId { get; set; }
        public int? CompetitiveGroupId { get; set; }
        public bool HasEge { get; set; }
        public decimal? EgeResultValue { get; set; }
        public int? AppealStatusId { get; set; }
        public bool? UsedInOrder { get; set; }

        public virtual AppealStatus AppealStatus { get; set; }
        public virtual Application Application { get; set; }
        public virtual Benefit Benefit { get; set; }
        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual EntranceTestItemC EntranceTestItem { get; set; }
        public virtual EntranceTestType EntranceTestType { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual InstitutionDocumentType InstitutionDocumentType { get; set; }
        public virtual EntranceTestResultSource Source { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
