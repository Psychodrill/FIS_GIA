using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BlkApplicationCompetitiveGroupItem
    {
        public string ApplicationUid { get; set; }
        public string CompetitivegroupUid { get; set; }
        public string CompetitiveGroupItemUid { get; set; }
        public int EducationForm { get; set; }
        public int EducationSource { get; set; }
        public int? Priority { get; set; }
        public string CompetitiveGroupTargetUid { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
