using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class LicensedDirectionStatus
    {
        public LicensedDirectionStatus()
        {
            LicensedDirection = new HashSet<LicensedDirection>();
        }

        public int StatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LicensedDirection> LicensedDirection { get; set; }
    }
}
