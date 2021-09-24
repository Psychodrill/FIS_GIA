using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class VCompetitiveGroup
    {
        public int CompetitiveGroupId { get; set; }
        public int InstitutionId { get; set; }
        public string Name { get; set; }
        public short Course { get; set; }
        public int? DirectionId { get; set; }
        public int? NumberBudgetO { get; set; }
        public int? NumberBudgetOz { get; set; }
        public int? NumberBudgetZ { get; set; }
        public int? NumberPaidO { get; set; }
        public int? NumberPaidOz { get; set; }
        public int? NumberPaidZ { get; set; }
        public int? NumberTargetO { get; set; }
        public int? NumberTargetOz { get; set; }
        public int? NumberTargetZ { get; set; }
        public string DirectionName { get; set; }
        public int? DirectionKey { get; set; }
        public int? CampaignId { get; set; }
        public int? EducationLevelId { get; set; }
    }
}
