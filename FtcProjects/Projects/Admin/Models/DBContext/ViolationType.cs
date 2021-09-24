using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ViolationType
    {
        public ViolationType()
        {
            Application = new HashSet<Application>();
        }

        public int ViolationId { get; set; }
        public string BriefName { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Application> Application { get; set; }
    }
}
