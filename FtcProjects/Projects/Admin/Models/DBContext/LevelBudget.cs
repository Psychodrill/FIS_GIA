using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class LevelBudget
    {
        public LevelBudget()
        {
            Application = new HashSet<Application>();
            ApplicationCompetitiveGroupItem = new HashSet<ApplicationCompetitiveGroupItem>();
            CompetitiveGroup = new HashSet<CompetitiveGroup>();
            DistributedAdmissionVolume = new HashSet<DistributedAdmissionVolume>();
            DistributedPlanAdmissionVolume = new HashSet<DistributedPlanAdmissionVolume>();
        }

        public int IdLevelBudget { get; set; }
        public string BudgetName { get; set; }

        public virtual ICollection<Application> Application { get; set; }
        public virtual ICollection<ApplicationCompetitiveGroupItem> ApplicationCompetitiveGroupItem { get; set; }
        public virtual ICollection<CompetitiveGroup> CompetitiveGroup { get; set; }
        public virtual ICollection<DistributedAdmissionVolume> DistributedAdmissionVolume { get; set; }
        public virtual ICollection<DistributedPlanAdmissionVolume> DistributedPlanAdmissionVolume { get; set; }
    }
}
