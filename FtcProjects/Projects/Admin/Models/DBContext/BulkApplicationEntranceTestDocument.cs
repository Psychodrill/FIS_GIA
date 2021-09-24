using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkApplicationEntranceTestDocument
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
        public decimal? ResultValue { get; set; }
        public short? EntranceTestTypeId { get; set; }
        public int? CompetitiveGroupId { get; set; }
        public int? EntranceTestItemId { get; set; }
        public int? SubjectId { get; set; }
        public int? SourceId { get; set; }
        public int? BenefitId { get; set; }
        public int? InstitutionDocumentTypeId { get; set; }
        public DateTime? InstitutionDocumentDate { get; set; }
        public string InstitutionDocumentNumber { get; set; }
        public string EgeDocumentId { get; set; }
        public Guid? BenefitEntrantDocumentId { get; set; }
        public int? EgeResultValue { get; set; }
        public Guid? EtentrantDocumentId { get; set; }
        public string DistantPlace { get; set; }
        public string DisabledDocumentUid { get; set; }
    }
}
