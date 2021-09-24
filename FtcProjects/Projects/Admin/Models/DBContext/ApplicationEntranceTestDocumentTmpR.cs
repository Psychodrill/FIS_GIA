using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationEntranceTestDocumentTmpR
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
    }
}
