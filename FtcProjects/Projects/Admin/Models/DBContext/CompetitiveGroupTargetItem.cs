using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CompetitiveGroupTargetItem
    {
        public int CompetitiveGroupTargetItemId { get; set; }
        public int CompetitiveGroupTargetId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? NumberTargetO { get; set; }
        public int? NumberTargetOz { get; set; }
        public int? NumberTargetZ { get; set; }
        public int CompetitiveGroupId { get; set; }

        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual CompetitiveGroupTarget CompetitiveGroupTarget { get; set; }
    }
}
