using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TempApps
    {
        public DateTime? CreateDate { get; set; }
        public int? PackageId { get; set; }
        public int? Apps { get; set; }
    }
}
