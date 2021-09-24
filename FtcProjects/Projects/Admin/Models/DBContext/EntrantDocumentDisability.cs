using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentDisability
    {
        public int EntrantDocumentId { get; set; }
        public int? DisabilityTypeId { get; set; }

        public virtual EntrantDocument EntrantDocument { get; set; }
    }
}
