using System;

namespace GVUZ.Util.Services.Parser
{
    public class ApplicationOrderRecordDto
    {
        public string ApplicationNumber { get; set; }
        public int? DirectionId { get; set; }
        public int? EducationFormId { get; set; }
        public int? EducationLevelId { get; set; }
        public int? FinanceSourceId { get; set; }
        public bool IsBeneficiary { get; set; }
        public bool IsForeigner { get; set; }
        public int? Stage { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public int InstitutionId { get; set; }

        public DateTime PackageCreatedDate { get; set; }
        public DateTime PackageModifiedDate { get; set; }
        public int PackageId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int? Status { get; set; }
        public string Comment { get; set; }
    }
}