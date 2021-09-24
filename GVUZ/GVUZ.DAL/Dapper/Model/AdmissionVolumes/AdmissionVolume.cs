using GVUZ.DAL.Dapper.Model.Campaigns;
using GVUZ.DAL.Dapper.Model.Dictionary;
using GVUZ.DAL.Dapper.Model.Directions;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.Model.Institutions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.AdmissionVolumes
{
    [Table("AdmissionVolume")]
    public partial class AdmissionVolume
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdmissionVolume()
        {
            DistributedAdmissionVolumes = new HashSet<DistributedAdmissionVolume>();
        }
        public int AdmissionVolumeID { get; set; }
        public int InstitutionID { get; set; }
        public short AdmissionItemTypeID { get; set; }
        public int? DirectionID { get; set; }
        public int NumberBudgetO { get; set; }
        public int NumberBudgetOZ { get; set; }
        public int NumberBudgetZ { get; set; }
        public int NumberPaidO { get; set; }
        public int NumberPaidOZ { get; set; }
        public int NumberPaidZ { get; set; }
        [StringLength(200)]
        public string UID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Course { get; set; }
        public int? CampaignID { get; set; }
        public int NumberTargetO { get; set; }
        public int NumberTargetOZ { get; set; }
        public int NumberTargetZ { get; set; }
        public int? NumberQuotaO { get; set; }
        public int? NumberQuotaOZ { get; set; }
        public int? NumberQuotaZ { get; set; }
        public Guid? AdmissionVolumeGUID { get; set; }
        public int? ParentDirectionID { get; set; }
        public virtual AdmissionItemType AdmissionItemType { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual Institution Institution { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DistributedAdmissionVolume> DistributedAdmissionVolumes { get; set; }
    }
}
