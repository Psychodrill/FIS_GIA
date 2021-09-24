using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class DistributedAdmissionVolume
    {
        public int DistributedAdmissionVolumeId { get; set; }
        public int AdmissionVolumeId { get; set; }
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

        public virtual AdmissionVolume AdmissionVolume { get; set; }
        public virtual LevelBudget IdLevelBudgetNavigation { get; set; }
    }
}
