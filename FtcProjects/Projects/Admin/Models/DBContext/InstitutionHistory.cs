using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionHistory
    {
        public int InstitutionHistoryId { get; set; }
        public short InstitutionTypeId { get; set; }
        public int InstitutionId { get; set; }
        public string FullName { get; set; }
        public string BriefName { get; set; }
        public int? FormOfLawId { get; set; }
        public int? RegionId { get; set; }
        public string Site { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public bool HasMilitaryDepartment { get; set; }
        public bool HasHostel { get; set; }
        public int? HostelCapacity { get; set; }
        public bool HasHostelForEntrants { get; set; }
        public int? HostelAttachmentId { get; set; }
        public string Inn { get; set; }
        public string Ogrn { get; set; }
        public DateTime? AdmissionStructurePublishDate { get; set; }
        public DateTime? ReceivingApplicationDate { get; set; }
        public int? EsrpOrgId { get; set; }
        public string OwnerDepartment { get; set; }
        public string Accreditation { get; set; }
        public int? AccreditationAttachmentId { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime? LicenseDate { get; set; }
        public int? LicenseAttachmentId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Attachment AccreditationAttachment { get; set; }
        public virtual FormOfLaw FormOfLaw { get; set; }
        public virtual Attachment HostelAttachment { get; set; }
        public virtual InstitutionType InstitutionType { get; set; }
        public virtual Attachment LicenseAttachment { get; set; }
        public virtual RegionType Region { get; set; }
    }
}
