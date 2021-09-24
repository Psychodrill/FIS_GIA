using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantLanguage
    {
        public int Id { get; set; }
        public int EntrantId { get; set; }
        public int LanguageId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Entrant1 Entrant { get; set; }
        public virtual ForeignLanguageType Language { get; set; }
    }
}
