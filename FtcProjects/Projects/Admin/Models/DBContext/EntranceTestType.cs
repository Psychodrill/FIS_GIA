using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntranceTestType
    {
        public EntranceTestType()
        {
            ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
            EntranceTestItemC = new HashSet<EntranceTestItemC>();
        }

        public short EntranceTestTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
        public virtual ICollection<EntranceTestItemC> EntranceTestItemC { get; set; }
    }
}
