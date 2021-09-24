using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkApplicationCompetitiveGroupItem
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int? CompetitiveGroupId { get; set; }
        public int? CompetitiveGroupItemId { get; set; }
        public int? CompetitiveGroupTargetId { get; set; }
        public int EducationForm { get; set; }
        public int EducationSource { get; set; }
        public int? Priority { get; set; }
        public DateTime? IsAgreedDate { get; set; }
        public bool? IsForSpoandVo { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
        public DateTime? IsDisagreedDate { get; set; }
    }
}
