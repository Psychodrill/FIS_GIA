using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkAdmissionVolume
    {
        public int? Id { get; set; }
        public Guid? Guid { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public int AdmissionItemTypeId { get; set; }
        public int DirectionId { get; set; }
        public int NumberBudgetO { get; set; }
        public int NumberBudgetOz { get; set; }
        public int NumberBudgetZ { get; set; }
        public int NumberPaidO { get; set; }
        public int NumberPaidOz { get; set; }
        public int NumberPaidZ { get; set; }
        public string Uid { get; set; }
        public int? Course { get; set; }
        public int? CampaignId { get; set; }
        public int NumberTargetO { get; set; }
        public int NumberTargetOz { get; set; }
        public int NumberTargetZ { get; set; }
        public int? NumberQuotaO { get; set; }
        public int? NumberQuotaOz { get; set; }
        public int? NumberQuotaZ { get; set; }
        public bool? IsPlan { get; set; }
    }
}
