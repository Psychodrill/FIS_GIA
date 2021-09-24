using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionLicenseStatus
    {
        public InstitutionLicenseStatus()
        {
            InstitutionLicenseSupplement = new HashSet<InstitutionLicenseSupplement>();
        }

        public int LicenseStatusId { get; set; }
        public string EiisId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<InstitutionLicenseSupplement> InstitutionLicenseSupplement { get; set; }
    }
}
