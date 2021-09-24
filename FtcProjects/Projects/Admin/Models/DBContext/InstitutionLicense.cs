using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Models.DBContext
{
    public partial class InstitutionLicense
    {
        public InstitutionLicense()
        {
            InstitutionLicenseSupplement = new HashSet<InstitutionLicenseSupplement>();
            LicensedDirection = new HashSet<LicensedDirection>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LicenseId { get; set; }
        public int InstitutionId { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseDate { get; set; }
        public int? AttachmentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string EiisId { get; set; }
        public int? EsrpId { get; set; }
        public virtual InstitutionAttachment InstitutionAttachment { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual ICollection<InstitutionLicenseSupplement> InstitutionLicenseSupplement { get; set; }
        public virtual ICollection<LicensedDirection> LicensedDirection { get; set; }
    }
}
