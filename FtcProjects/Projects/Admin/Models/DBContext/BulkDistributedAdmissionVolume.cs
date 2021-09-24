using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkDistributedAdmissionVolume
    {
        public int? Id { get; set; }
        public Guid? Guid { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public int? AdmissionVolumeId { get; set; }
        public Guid? AdmissionVolumeGuid { get; set; }
        public int IdLevelBudget { get; set; }
        public int NumberBudgetO { get; set; }
        public int NumberBudgetOz { get; set; }
        public int NumberBudgetZ { get; set; }
        public int NumberQuotaO { get; set; }
        public int NumberQuotaOz { get; set; }
        public int NumberQuotaZ { get; set; }
        public int NumberTargetO { get; set; }
        public int NumberTargetOz { get; set; }
        public int NumberTargetZ { get; set; }
        public bool? IsPlan { get; set; }
    }
}
