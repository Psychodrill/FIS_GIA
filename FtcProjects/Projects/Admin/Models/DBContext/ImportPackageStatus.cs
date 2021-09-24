using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ImportPackageStatus
    {
        public ImportPackageStatus()
        {
            ImportPackage = new HashSet<ImportPackage>();
        }

        public int StatusId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<ImportPackage> ImportPackage { get; set; }
    }
}
