using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntranceTestItemC
    {
        public EntranceTestItemC()
        {
            ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
            BenefitItemC = new HashSet<BenefitItemC>();
            InverseReplacedEntranceTestItem = new HashSet<EntranceTestItemC>();
        }

        public int EntranceTestItemId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public short EntranceTestTypeId { get; set; }
        public decimal? MinScore { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? EntranceTestPriority { get; set; }
        public Guid? EntranceTestItemGuid { get; set; }
        public bool? IsForSpoandVo { get; set; }
        public int? ReplacedEntranceTestItemId { get; set; }

        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual EntranceTestType EntranceTestType { get; set; }
        public virtual EntranceTestItemC ReplacedEntranceTestItem { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
        public virtual ICollection<BenefitItemC> BenefitItemC { get; set; }
        public virtual ICollection<EntranceTestItemC> InverseReplacedEntranceTestItem { get; set; }
    }
}
