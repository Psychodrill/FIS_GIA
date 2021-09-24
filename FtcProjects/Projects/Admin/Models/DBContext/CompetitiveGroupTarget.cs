using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CompetitiveGroupTarget
    {
        public CompetitiveGroupTarget()
        {
            Application = new HashSet<Application>();
            CompetitiveGroupTargetItem = new HashSet<CompetitiveGroupTargetItem>();
        }

        public int CompetitiveGroupTargetId { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int InstitutionId { get; set; }

        public virtual Institution Institution { get; set; }
        public virtual ICollection<Application> Application { get; set; }
        public virtual ICollection<CompetitiveGroupTargetItem> CompetitiveGroupTargetItem { get; set; }
    }
}
