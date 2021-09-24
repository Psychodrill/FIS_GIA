using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AppealStatus
    {
        public AppealStatus()
        {
            ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
        }

        public int AppealStatusId { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
    }
}
