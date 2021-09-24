using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationForcedAdmissionDocument
    {
        public int ApplicationForcedAdmissionDocumentId { get; set; }
        public int ApplicationId { get; set; }
        public int AttachmentId { get; set; }
        public int DocumentType { get; set; }

        public virtual Application Application { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
