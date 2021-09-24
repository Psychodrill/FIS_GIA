using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionDocuments
    {
        public int InstitutionId { get; set; }
        public int AttachmentId { get; set; }
        public int Year { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual Institution Institution { get; set; }
    }
}
