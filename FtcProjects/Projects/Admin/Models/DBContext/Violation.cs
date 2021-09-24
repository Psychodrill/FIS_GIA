using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Violation
    {
        public int ViolationId { get; set; }
        public int EntrantDocumentId { get; set; }
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int CountryId { get; set; }
        public int DocumentTypeId { get; set; }
        public short EducationLevelId { get; set; }
        public short EducationFormId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string DocumentOrganization { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public int? BlankSeries { get; set; }
        public int? BlankNumber { get; set; }
        public string BlankRegNumber { get; set; }
        public DateTime? BlankDate { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual CountryType Country { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual AdmissionItemType EducationForm { get; set; }
        public virtual AdmissionItemType EducationLevel { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
    }
}
