using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkEntrantDocument
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public bool OriginalReceived { get; set; }
        public DateTime? OriginalReceivedDate { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentOrganization { get; set; }
        public string DocumentSeries { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentSpecificData { get; set; }
        public int? IdentityDocumentTypeId { get; set; }
        public int? NationalityTypeId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? EndYear { get; set; }
        public string BirthPlace { get; set; }
        public string SubdivisionCode { get; set; }
        public string RegistrationNumber { get; set; }
        public string QualificationName { get; set; }
        public string SpecialityName { get; set; }
        public int? SpecializationId { get; set; }
        public int? ProfessionId { get; set; }
        public string DocumentTypeNameText { get; set; }
        public string AdditionalInfo { get; set; }
        public int? DisabilityTypeId { get; set; }
        public int? DiplomaTypeId { get; set; }
        public int? OlympicId { get; set; }
        public string OlympicPlace { get; set; }
        public DateTime? OlympicDate { get; set; }
        public int? ProfileId { get; set; }
        public int? ClassNumber { get; set; }
        public string OlympicName { get; set; }
        public string OlympicProfile { get; set; }
        public int? SportCategoryId { get; set; }
        public int? OrphanCategoryId { get; set; }
        public int? CompatriotCategoryId { get; set; }
        public int? CountryId { get; set; }
        public Guid? EntranceTestResultId { get; set; }
        public float? Gpa { get; set; }
        public int? EntrantDocumentId { get; set; }
        public int? VeteranCategoryId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int? ProfileSubjectId { get; set; }
        public int? EgeSubjectId { get; set; }
        public int? ParentsLostCategoryId { get; set; }
        public int? StateEmployeeCategoryId { get; set; }
        public int? RadiationWorkCategoryId { get; set; }
        public bool? StateServicePreparation { get; set; }
        public bool? IsNostrificated { get; set; }
    }
}
