using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BlkApplicationEntranceTestDocument
    {
        public decimal? ResultValue { get; set; }
        public short? EntranceTestTypeId { get; set; }
        public string CompetitiveGroupUid { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int? SourceId { get; set; }
        public int? BenefitId { get; set; }
        public int? InstitutionDocumentTypeId { get; set; }
        public DateTime? InstitutionDocumentDate { get; set; }
        public string InstitutionDocumentNumber { get; set; }
        public string EgeDocumentId { get; set; }
        public Guid? BenefitEntrantDocumentId { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
