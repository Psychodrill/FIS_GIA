using System;
using GVUZ.ServiceModel.Import.Bulk.Attributes;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_ApplicationEntranceTestDocument")]
    public class AppEntranceTestDocumentBulkEntity : BulkEntityBase
    {
        public decimal? ResultValue { get; set; }
        public int? EntranceTestTypeId { get; set; }
        public string CompetitiveGroupUID { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int? SourceId { get; set; }
        public int? BenefitId { get; set; }
        public int? InstitutionDocumentTypeId { get; set; }
        public DateTime? InstitutionDocumentDate { get; set; }
        public string InstitutionDocumentNumber { get; set; }
        public string EgeDocumentId { get; set; }
        public Guid? BenefitEntrantDocumentId { get; set; }
    }
}
