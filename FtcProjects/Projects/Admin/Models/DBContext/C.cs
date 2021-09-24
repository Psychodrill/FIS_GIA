using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class C
    {
        public int? InstitutionId { get; set; }
        public int? YearStart { get; set; }
        public int? CampaignId { get; set; }
        public string CampaignName { get; set; }
        public int? CompetitiveGroupId { get; set; }
        public string CompetitiveGroupName { get; set; }
        public int? Course { get; set; }
        public int? CompetitiveGroupItemId { get; set; }
        public int? CompetitiveGroupTargetId { get; set; }
        public string CompetitiveGroupTargetName { get; set; }
        public int? DirectionId { get; set; }
        public string DirectionCode { get; set; }
        public string DirectionName { get; set; }
        public int? EducationLevelId { get; set; }
        public string EducationLevelName { get; set; }
        public int? EducationFormId { get; set; }
        public string EducationFormName { get; set; }
        public int? EducationSourceId { get; set; }
        public string EducationSourceName { get; set; }
        public string ConditionCode { get; set; }
        public int? Places { get; set; }
        public int? PlacesBase { get; set; }
    }
}
