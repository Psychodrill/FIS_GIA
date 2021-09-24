using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GetInstitutionCampaign
    {
        public int InstitutionId { get; set; }
        public string BriefName { get; set; }
        public string FullName { get; set; }
        public string EiisId { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string Ogrn { get; set; }
        public int CampaignId { get; set; }
        public string Uid { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public int EduLevelTypeId { get; set; }
        public string EduLevelName { get; set; }
        public short CampaignTypeId { get; set; }
        public string CampageTypeName { get; set; }
        public int DirectionId { get; set; }
        public string NewCode { get; set; }
        public string ParentName { get; set; }
        public string DirectName { get; set; }
        public int NumberBudgetO { get; set; }
        public int NumberBudgetOz { get; set; }
        public int NumberBudgetZ { get; set; }
        public int NumberPaidO { get; set; }
        public int NumberPaidOz { get; set; }
        public int NumberPaidZ { get; set; }
        public int NumberTargetO { get; set; }
        public int NumberTargetOz { get; set; }
        public int NumberTargetZ { get; set; }
        public int? NumberQuotaO { get; set; }
        public int? NumberQuotaOz { get; set; }
        public int? NumberQuotaZ { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
    }
}
