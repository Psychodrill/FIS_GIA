using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentMarks
    {
        public int EntrantDocumentId { get; set; }
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
