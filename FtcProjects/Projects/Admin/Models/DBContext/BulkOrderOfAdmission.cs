using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkOrderOfAdmission
    {
        public int? Id { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
        public Guid Guid { get; set; }
        public string Uid { get; set; }
        public int? ApplicationId { get; set; }
        public int? ApplicationLevelBudgetId { get; set; }
        public int? ApplicationCgitemId { get; set; }
        public int? DirectionId { get; set; }
        public int? EducationFormId { get; set; }
        public int? EducationLevelId { get; set; }
        public int? FinanceSourceId { get; set; }
        public bool IsBeneficiary { get; set; }
        public bool IsForeigner { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? OrderDatePublished { get; set; }
        public string OrderName { get; set; }
        public string OrderNumber { get; set; }
        public int? Stage { get; set; }
        public int? CampaignId { get; set; }
        public int? Course { get; set; }
        public int OrderStatus { get; set; }
    }
}
