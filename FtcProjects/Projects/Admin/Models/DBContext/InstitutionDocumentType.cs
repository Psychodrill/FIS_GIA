using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionDocumentType
    {
        public InstitutionDocumentType()
        {
            ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
        }

        public int InstitutionDocumentTypeId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }

        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
    }
}
