using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class LicensedDirection
    {
        public int Ldid { get; set; }
        public int LicenseId { get; set; }
        public int DirectionId { get; set; }
        public int StatusId { get; set; }
        public short? EduLevelId { get; set; }
        public int LicSupplementId { get; set; }

        public virtual Direction Direction { get; set; }
        public virtual AdmissionItemType EduLevel { get; set; }
        public virtual InstitutionLicenseSupplement LicSupplement { get; set; }
        public virtual InstitutionLicense License { get; set; }
        public virtual LicensedDirectionStatus Status { get; set; }
    }
}
