using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocument
    {
        public EntrantDocument()
        {
            Application = new HashSet<Application>();
            ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
            ApplicationEntrantDocument = new HashSet<ApplicationEntrantDocument>();
            Entrant1IdentityDocument = new HashSet<Entrant1>();
            Entrant1IsFromKrymEntrantDocument = new HashSet<Entrant1>();
            IndividualAchivement = new HashSet<IndividualAchivement>();
        }

        public int EntrantDocumentId { get; set; }
        public int? EntrantId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentOrganization { get; set; }
        public string DocumentSpecificData { get; set; }
        public int? AttachmentId { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? EntrantDocumentGuid { get; set; }
        public bool? OlympApproved { get; set; }
        public string DocumentName { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual Entrant1 Entrant { get; set; }
        public virtual Violation Violation { get; set; }
        public virtual ICollection<Application> Application { get; set; }
        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
        public virtual ICollection<ApplicationEntrantDocument> ApplicationEntrantDocument { get; set; }
        public virtual ICollection<Entrant1> Entrant1IdentityDocument { get; set; }
        public virtual ICollection<Entrant1> Entrant1IsFromKrymEntrantDocument { get; set; }
        public virtual ICollection<IndividualAchivement> IndividualAchivement { get; set; }
    }
}
