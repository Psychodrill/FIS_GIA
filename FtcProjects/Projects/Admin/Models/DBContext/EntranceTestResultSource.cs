using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntranceTestResultSource
    {
        public EntranceTestResultSource()
        {
            ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
        }

        public int SourceId { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
    }
}
