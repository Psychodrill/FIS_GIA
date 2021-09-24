using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.LevelBudgets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes
{
    [Table("DistributedAdmissionVolume")]
    public partial class DistributedAdmissionVolume
    {
        public int DistributedAdmissionVolumeID { get; set; }

        public int AdmissionVolumeID { get; set; }

        public int IdLevelBudget { get; set; }

        public int NumberBudgetO { get; set; }

        public int NumberBudgetOZ { get; set; }

        public int NumberBudgetZ { get; set; }

        public int NumberQuotaO { get; set; }

        public int NumberQuotaOZ { get; set; }

        public int NumberQuotaZ { get; set; }

        public int NumberTargetO { get; set; }

        public int NumberTargetOZ { get; set; }

        public int NumberTargetZ { get; set; }

        public virtual AdmissionVolume AdmissionVolume { get; set; }

        public virtual LevelBudget LevelBudget { get; set; }
    }
}
