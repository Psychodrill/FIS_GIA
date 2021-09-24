using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentOrphan
    {
        public int EntrantDocumentId { get; set; }
        public int OrphanCategoryId { get; set; }

        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual OrphanCategory OrphanCategory { get; set; }
    }
}
