using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class A
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
        public int? Places { get; set; }
        public int AcgiId { get; set; }
        public int? ApplicationId { get; set; }
        public int? EntrantId { get; set; }
        public string Fio { get; set; }
        public bool? OriginalDocumentsReceived { get; set; }
        public string ConditionCode { get; set; }
        public int? ApplicationPriority { get; set; }
        public int? AcgiPriority { get; set; }
        public int? CalcPriority { get; set; }
        public int? Mark { get; set; }
        public int? MarkIa { get; set; }
        public bool? BezVi { get; set; }
        public bool? OsoboePravo { get; set; }
        public bool? PreimPravo { get; set; }
        public int? ARank { get; set; }
        public string SRank { get; set; }
        public string IncludeTo { get; set; }
        public int? Excluded { get; set; }
    }
}
