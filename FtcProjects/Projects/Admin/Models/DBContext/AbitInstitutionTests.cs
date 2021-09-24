using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AbitInstitutionTests
    {
        public int InstitutionId { get; set; }
        public int YearStart { get; set; }
        public string Level { get; set; }
        public int SpecId { get; set; }
        public string SpecCode { get; set; }
        public string SpecName { get; set; }
        public int BudgetO { get; set; }
        public int BudgetOz { get; set; }
        public int BudgetZ { get; set; }
        public int PaidO { get; set; }
        public int PaidOz { get; set; }
        public int PaidZ { get; set; }
        public int QuotaO { get; set; }
        public int QuotaOz { get; set; }
        public int QuotaZ { get; set; }
        public int TargetO { get; set; }
        public int TargetOz { get; set; }
        public int TargetZ { get; set; }
        public string TestType { get; set; }
        public string Subject { get; set; }
        public int? MinScore { get; set; }
        public short LevelId { get; set; }
        public int? SubjectId { get; set; }
        public decimal? MaxScore { get; set; }
        public int? MinScoreRon { get; set; }
    }
}
