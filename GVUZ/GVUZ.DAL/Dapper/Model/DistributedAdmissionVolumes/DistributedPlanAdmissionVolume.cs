using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes
{
    [Table("DistributedPlanAdmissionVolume")]
    public class DistributedPlanAdmissionVolume
    {
        public int DistributedPlanAdmissionVolumeID { get; set; }
        public int PlanAdmissionVolumeID { get; set; }
        public int IdLevelBudget { get; set; }
        public int Number { get; set; }
    }
}
