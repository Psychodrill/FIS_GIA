using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentVeteran
    {
        public int EntrantDocumentId { get; set; }
        public int VeteranCategoryId { get; set; }

        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual VeteranCategory VeteranCategory { get; set; }
    }
}
