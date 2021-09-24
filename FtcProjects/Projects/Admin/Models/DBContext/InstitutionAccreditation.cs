using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionAccreditation
    {
        public int AccreditationId { get; set; }
        public int InstitutionId { get; set; }
        public string Accreditation { get; set; }
        public int? AttachmentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual InstitutionAttachment InstitutionAttachment { get; set; }
        public virtual Institution Institution { get; set; }
    }
}
