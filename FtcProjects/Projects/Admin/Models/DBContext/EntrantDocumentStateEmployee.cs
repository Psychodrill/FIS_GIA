using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentStateEmployee
    {
        public int EntrantDocumentId { get; set; }
        public int StateEmployeeCategoryId { get; set; }

        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual StateEmployeeCategory StateEmployeeCategory { get; set; }
    }
}
