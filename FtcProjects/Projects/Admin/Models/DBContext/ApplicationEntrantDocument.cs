using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationEntrantDocument
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int EntrantDocumentId { get; set; }
        public int? AttachedDocumentType { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? OriginalReceivedDate { get; set; }

        public virtual Application Application { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
    }
}
