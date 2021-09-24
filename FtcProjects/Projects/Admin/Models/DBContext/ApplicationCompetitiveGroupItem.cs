using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationCompetitiveGroupItem
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public int? CompetitiveGroupItemId { get; set; }
        public int EducationFormId { get; set; }
        public int EducationSourceId { get; set; }
        public int? Priority { get; set; }
        public int? CompetitiveGroupTargetId { get; set; }
        public bool? IsAgreed { get; set; }
        public bool? IsDisagreed { get; set; }
        public bool? IsForSpoandVo { get; set; }
        public DateTime? IsAgreedDate { get; set; }
        public DateTime? IsDisagreedDate { get; set; }
        public decimal? CalculatedRating { get; set; }
        public int? OrderOfAdmissionId { get; set; }
        public int? OrderOfExceptionId { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? ExceptionDate { get; set; }
        public short? OrderBenefitId { get; set; }
        public int? OrderIdLevelBudget { get; set; }

        public virtual Application Application { get; set; }
        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual CompetitiveGroupItem CompetitiveGroupItem { get; set; }
        public virtual Benefit OrderBenefit { get; set; }
        public virtual LevelBudget OrderIdLevelBudgetNavigation { get; set; }
        public virtual OrderOfAdmission OrderOfAdmission { get; set; }
        public virtual OrderOfAdmission OrderOfException { get; set; }
    }
}
