using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class DocumentType
    {
        public DocumentType()
        {
            CountryDocuments = new HashSet<CountryDocuments>();
            EduLevelDocumentType = new HashSet<EduLevelDocumentType>();
            EntrantDocument = new HashSet<EntrantDocument>();
            Violation = new HashSet<Violation>();
        }

        public int DocumentId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int CategoryId { get; set; }
        public bool IsMedical { get; set; }

        public virtual DocumentCategory Category { get; set; }
        public virtual ICollection<CountryDocuments> CountryDocuments { get; set; }
        public virtual ICollection<EduLevelDocumentType> EduLevelDocumentType { get; set; }
        public virtual ICollection<EntrantDocument> EntrantDocument { get; set; }
        public virtual ICollection<Violation> Violation { get; set; }
    }
}
