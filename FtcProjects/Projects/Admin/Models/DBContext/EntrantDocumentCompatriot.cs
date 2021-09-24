using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentCompatriot
    {
        public int EntrantDocumentId { get; set; }
        public int CompatriotCategoryId { get; set; }

        public virtual CompatriotCategory CompatriotCategory { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
    }
}
