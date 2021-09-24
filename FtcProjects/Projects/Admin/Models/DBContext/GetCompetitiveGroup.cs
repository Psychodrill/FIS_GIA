using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GetCompetitiveGroup
    {
        public int? CampaignId { get; set; }
        public int InstitutionId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public short? EducationFormId { get; set; }
        public string EducationFormName { get; set; }
        public short? EducationSourceId { get; set; }
        public string EducationSourceName { get; set; }
        public short? EducationLevelId { get; set; }
        public string EducationLevelName { get; set; }
        public int? DirectionId { get; set; }
        public string NewCode { get; set; }
        public string ParentName { get; set; }
        public string DirectName { get; set; }
        public int? NumberBudgetO { get; set; }
        public int? NumberBudgetOz { get; set; }
        public int? NumberBudgetZ { get; set; }
        public int? NumberPaidO { get; set; }
        public int? NumberPaidOz { get; set; }
        public int? NumberPaidZ { get; set; }
        public int? NumberTargetO { get; set; }
        public int? NumberTargetOz { get; set; }
        public int? NumberTargetZ { get; set; }
        public int? NumberQuotaO { get; set; }
        public int? NumberQuotaOz { get; set; }
        public int? NumberQuotaZ { get; set; }
    }
}
