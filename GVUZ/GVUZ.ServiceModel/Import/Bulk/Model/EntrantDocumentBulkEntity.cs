using System;
using GVUZ.ServiceModel.Import.Bulk.Attributes;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_EntrantDocument")]
    public class EntrantDocumentBulkEntity : BulkEntityBase
    {
        public bool OriginalReceived { get; set; }
        public DateTime? OriginalReceivedDate { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentOrganization { get; set; }
        public string DocumentSeries { get; set; }
        public int DocumentTypeId { get; set; }
        public int? IdentityDocumentTypeId { get; set; }
        public int? NationalityTypeId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? EndYear { get; set; }
        public string DocumentSpecificData { get; set; }
        public string BirthPlace { get; set; }
        public string SubdivisionCode { get; set; }
        public string RegistrationNumber { get; set; }
        public int? QualificationTypeId { get; set; }
        public int? SpecialityId { get; set; }
        public int? SpecializationId { get; set; }
        public int? ProfessionId { get; set; }
        public string DocumentTypeNameText { get; set; }
        public string AdditionalInfo { get; set; }
        public int? DisabilityTypeId { get; set; }
        public int? DiplomaTypeId { get; set; }
        public int? OlympicId { get; set; }
        public string OlympicPlace { get; set; }
        public DateTime? OlympicDate { get; set; }
        public Guid? EntranceTestResultId { get; set; }
        public float? GPA { get; set; }
        //public bool? StateServicePreparation { get; set; }
    }
}
