using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TmpOlympicDiplomantDocument
    {
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
    }
}
