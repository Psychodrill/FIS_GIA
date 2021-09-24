using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class IdentityDocumentType
    {
        public IdentityDocumentType()
        {
            OlympicDiplomantDocument = new HashSet<OlympicDiplomantDocument>();
            TranslationRvidtIdentityDt = new HashSet<TranslationRvidtIdentityDt>();
        }

        public int IdentityDocumentTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsRussianNationality { get; set; }

        public virtual ICollection<OlympicDiplomantDocument> OlympicDiplomantDocument { get; set; }
        public virtual ICollection<TranslationRvidtIdentityDt> TranslationRvidtIdentityDt { get; set; }
    }
}
