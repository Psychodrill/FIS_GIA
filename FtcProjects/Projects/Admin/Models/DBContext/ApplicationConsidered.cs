using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationConsidered
    {
        public int ApplicationConsideredId { get; set; }
        public int ApplicationId { get; set; }
        public int DirectionId { get; set; }
        public short EducationLevelId { get; set; }
        public short EducationFormId { get; set; }
        public short FinanceSourceId { get; set; }
        public bool IsRequiresBudgetO { get; set; }
        public bool IsRequiresBudgetOz { get; set; }
        public bool IsRequiresBudgetZ { get; set; }
        public bool IsRequiresPaidO { get; set; }
        public bool IsRequiresPaidOz { get; set; }
        public bool IsRequiresPaidZ { get; set; }
        public bool IsRequiresTargetO { get; set; }
        public bool IsRequiresTargetOz { get; set; }
        public bool IsRequiresTargetZ { get; set; }
        public short? Stage { get; set; }
        public bool IsRecommended { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Application Application { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual AdmissionItemType EducationLevel { get; set; }
    }
}
