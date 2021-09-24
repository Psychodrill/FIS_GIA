using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class DistributedPlanAdmissionVolume
    {
        public int DistributedPlanAdmissionVolumeId { get; set; }
        public int PlanAdmissionVolumeId { get; set; }
        public int IdLevelBudget { get; set; }
        public int Number { get; set; }

        public virtual LevelBudget IdLevelBudgetNavigation { get; set; }
        public virtual PlanAdmissionVolume PlanAdmissionVolume { get; set; }
    }
}
