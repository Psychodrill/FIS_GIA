using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.LevelBudgets
{
    [Table("LevelBudget")]
    public partial class LevelBudget
    {
        public LevelBudget()
        {
            //Applications = new HashSet<Application>();
            DistributedAdmissionVolumes = new HashSet<DistributedAdmissionVolume>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdLevelBudget { get; set; }

        [Required]
        [StringLength(500)]
        public string BudgetName { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Application> Applications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DistributedAdmissionVolume> DistributedAdmissionVolumes { get; set; }
    }
}
