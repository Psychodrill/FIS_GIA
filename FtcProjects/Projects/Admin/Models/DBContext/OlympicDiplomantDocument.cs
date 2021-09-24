using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicDiplomantDocument
    {
        public OlympicDiplomantDocument()
        {
            OlympicDiplomant = new HashSet<OlympicDiplomant>();
        }

        public long OlympicDiplomantDocumentId { get; set; }
        public long OlympicDiplomantId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int IdentityDocumentTypeId { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public string OrganizationIssue { get; set; }
        public DateTime? DateIssue { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public virtual IdentityDocumentType IdentityDocumentType { get; set; }
        public virtual OlympicDiplomant OlympicDiplomantNavigation { get; set; }
        public virtual ICollection<OlympicDiplomant> OlympicDiplomant { get; set; }
    }
}
