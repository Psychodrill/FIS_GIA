using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentRadiationWork
    {
        public int EntrantDocumentId { get; set; }
        public int RadiationWorkCategoryId { get; set; }

        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual RadiationWorkCategory RadiationWorkCategory { get; set; }
    }
}
