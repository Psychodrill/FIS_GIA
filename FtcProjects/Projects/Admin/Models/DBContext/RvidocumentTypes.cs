using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class RvidocumentTypes
    {
        public RvidocumentTypes()
        {
            RvipersonIdentDocs = new HashSet<RvipersonIdentDocs>();
            TranslationRvidtIdentityDt = new HashSet<TranslationRvidtIdentityDt>();
        }

        public int DocumentTypeCode { get; set; }
        public string DocumentTypeName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<RvipersonIdentDocs> RvipersonIdentDocs { get; set; }
        public virtual ICollection<TranslationRvidtIdentityDt> TranslationRvidtIdentityDt { get; set; }
    }
}
