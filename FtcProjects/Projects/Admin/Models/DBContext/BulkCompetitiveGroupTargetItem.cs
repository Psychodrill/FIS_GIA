using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkCompetitiveGroupTargetItem
    {
        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public int NumberTargetO { get; set; }
        public int NumberTargetOz { get; set; }
        public int NumberTargetZ { get; set; }
        public int TargetId { get; set; }
        public int? CompetitiveGroupId { get; set; }
        public Guid? CompetitiveGroupGuid { get; set; }
    }
}
