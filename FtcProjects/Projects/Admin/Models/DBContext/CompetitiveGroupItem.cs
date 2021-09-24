using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CompetitiveGroupItem
    {
        public CompetitiveGroupItem()
        {
            ApplicationCompetitiveGroupItem = new HashSet<ApplicationCompetitiveGroupItem>();
        }

        public int CompetitiveGroupItemId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public int? NumberBudgetO { get; set; }
        public int? NumberBudgetOz { get; set; }
        public int? NumberBudgetZ { get; set; }
        public int? NumberPaidO { get; set; }
        public int? NumberPaidOz { get; set; }
        public int? NumberPaidZ { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? NumberQuotaO { get; set; }
        public int? NumberQuotaOz { get; set; }
        public int? NumberQuotaZ { get; set; }
        public int? NumberTargetO { get; set; }
        public int? NumberTargetOz { get; set; }
        public int? NumberTargetZ { get; set; }

        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual ICollection<ApplicationCompetitiveGroupItem> ApplicationCompetitiveGroupItem { get; set; }
    }
}
