using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentCustom
    {
        public int EntrantDocumentId { get; set; }
        public string AdditionalInfo { get; set; }

        public virtual EntrantDocument EntrantDocument { get; set; }
    }
}
