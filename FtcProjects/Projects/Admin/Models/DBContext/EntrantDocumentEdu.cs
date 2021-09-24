using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentEdu
    {
        public int EntrantDocumentId { get; set; }
        public string RegistrationNumber { get; set; }
        public string InstitutionName { get; set; }
        public string SpecialityName { get; set; }
        public string QualificationName { get; set; }
        public float? Gpa { get; set; }
        public bool? IsNostrificated { get; set; }
        public bool? StateServicePreparation { get; set; }
        public int? BlankSeries { get; set; }
        public int? BlankNumber { get; set; }
        public string BlankRegNumber { get; set; }
        public DateTime? BlankDate { get; set; }
        public int? BlankAttachmentId { get; set; }
        public string NostrificatedComment { get; set; }
        public int? NostrificationTypeId { get; set; }
        public int? CountryId { get; set; }
        public bool FrdoChecked { get; set; }
        public bool FrdoIsGenuine { get; set; }
        public bool FrdoLossConfirmed { get; set; }
        public bool FrdoExchangeConfirmed { get; set; }
        public string FrdoDocName { get; set; }
        public string FrdoOrganization { get; set; }
        public string FrdoQualification { get; set; }
        public string FrdoSpeciality { get; set; }
        public bool HasBlank { get; set; }
        public bool? IsNostrificatedInInstitution { get; set; }
        public string ActNumber { get; set; }
        public DateTime? ActDate { get; set; }
        public string ActComment { get; set; }
        public string AdditionalLastName { get; set; }
        public string AdditionalFirstName { get; set; }
        public string AdditionalMiddleName { get; set; }

        public virtual Attachment BlankAttachment { get; set; }
        public virtual CountryType Country { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual NostrificationTypes NostrificationType { get; set; }
    }
}
