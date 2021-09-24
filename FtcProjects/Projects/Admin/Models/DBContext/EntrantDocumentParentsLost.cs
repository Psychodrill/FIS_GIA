using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentParentsLost
    {
        public int EntrantDocumentId { get; set; }
        public int ParentsLostCategoryId { get; set; }

        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual ParentsLostCategory ParentsLostCategory { get; set; }
    }
}
