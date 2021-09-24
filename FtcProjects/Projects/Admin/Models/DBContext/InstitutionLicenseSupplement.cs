using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionLicenseSupplement
    {
        public InstitutionLicenseSupplement()
        {
            LicensedDirection = new HashSet<LicensedDirection>();
        }

        public int LicSupplementId { get; set; }
        public int LicenseId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string RegNumber { get; set; }
        public int StatusId { get; set; }
        public string EiisId { get; set; }

        public virtual InstitutionLicense License { get; set; }
        public virtual InstitutionLicenseStatus Status { get; set; }
        public virtual ICollection<LicensedDirection> LicensedDirection { get; set; }
    }
}
