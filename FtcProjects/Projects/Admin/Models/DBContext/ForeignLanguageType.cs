using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ForeignLanguageType
    {
        public ForeignLanguageType()
        {
            EntrantLanguage = new HashSet<EntrantLanguage>();
        }

        public int LanguageId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<EntrantLanguage> EntrantLanguage { get; set; }
    }
}
