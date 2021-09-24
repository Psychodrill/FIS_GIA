using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AbitInstitutionVolAndStruct
    {
        public int InstitutionId { get; set; }
        public int YearStart { get; set; }
        public string Level { get; set; }
        public int SpecId { get; set; }
        public string SpecCode { get; set; }
        public string SpecName { get; set; }
        public string NewSpecCode { get; set; }
        public int AppBudgetO { get; set; }
        public int AppBudgetOz { get; set; }
        public int AppBudgetZ { get; set; }
        public int AppTargetO { get; set; }
        public int AppTargetOz { get; set; }
        public int AppTargetZ { get; set; }
        public int AppQuotaO { get; set; }
        public int AppQuotaOz { get; set; }
        public int AppQuotaZ { get; set; }
        public int AppPaidO { get; set; }
        public int AppPaidOz { get; set; }
        public int AppPaidZ { get; set; }
        public int? BudgetO { get; set; }
        public int? BudgetOz { get; set; }
        public int? BudgetZ { get; set; }
        public int? TargetO { get; set; }
        public int? TargetOz { get; set; }
        public int? TargetZ { get; set; }
        public int? QuotaO { get; set; }
        public int? QuotaOz { get; set; }
        public int? QuotaZ { get; set; }
        public int? PaidO { get; set; }
        public int? PaidOz { get; set; }
        public int? PaidZ { get; set; }
        public int ContestBudgetO { get; set; }
        public int ContestBudgetOz { get; set; }
        public int ContestBudgetZ { get; set; }
        public int ContestTargetO { get; set; }
        public int ContestTargetOz { get; set; }
        public int ContestTargetZ { get; set; }
        public int ContestQuotaO { get; set; }
        public int ContestQuotaOz { get; set; }
        public int ContestQuotaZ { get; set; }
        public int ContestPaidO { get; set; }
        public int ContestPaidOz { get; set; }
        public int ContestPaidZ { get; set; }
        public int EnrolledBudgetO { get; set; }
        public int EnrolledBudgetOz { get; set; }
        public int EnrolledBudgetZ { get; set; }
        public int EnrolledTargetO { get; set; }
        public int EnrolledTargetOz { get; set; }
        public int EnrolledTargetZ { get; set; }
        public int EnrolledQuotaO { get; set; }
        public int EnrolledQuotaOz { get; set; }
        public int EnrolledQuotaZ { get; set; }
        public int EnrolledPaidO { get; set; }
        public int EnrolledPaidOz { get; set; }
        public int EnrolledPaidZ { get; set; }
        public int CurrentScoreBudgetO { get; set; }
        public int CurrentScoreBudgetOz { get; set; }
        public int CurrentScoreBudgetZ { get; set; }
        public int CurrentScoreTargetO { get; set; }
        public int CurrentScoreTargetOz { get; set; }
        public int CurrentScoreTargetZ { get; set; }
        public int CurrentScoreQuotaO { get; set; }
        public int CurrentScoreQuotaOz { get; set; }
        public int CurrentScoreQuotaZ { get; set; }
        public int CurrentScorePaidO { get; set; }
        public int CurrentScorePaidOz { get; set; }
        public int CurrentScorePaidZ { get; set; }
        public int ParentDirId { get; set; }
        public string ParentDirCode { get; set; }
        public string ParentDirName { get; set; }
        public short LevelId { get; set; }
    }
}
