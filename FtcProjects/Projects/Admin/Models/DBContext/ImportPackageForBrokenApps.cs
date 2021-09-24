using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ImportPackageForBrokenApps
    {
        public int Institutionid { get; set; }
        public int Packageid { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
