using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AdmissionVolume
    {
        public AdmissionVolume()
        {
            DistributedAdmissionVolume = new HashSet<DistributedAdmissionVolume>();
        }

        public int AdmissionVolumeId { get; set; }
        public int InstitutionId { get; set; }
        public short AdmissionItemTypeId { get; set; }
        public int DirectionId { get; set; }
        public int NumberBudgetO { get; set; }
        public int NumberBudgetOz { get; set; }
        public int NumberBudgetZ { get; set; }
        public int NumberPaidO { get; set; }
        public int NumberPaidOz { get; set; }
        public int NumberPaidZ { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Course { get; set; }
        public int? CampaignId { get; set; }
        public int NumberTargetO { get; set; }
        public int NumberTargetOz { get; set; }
        public int NumberTargetZ { get; set; }
        public int? NumberQuotaO { get; set; }
        public int? NumberQuotaOz { get; set; }
        public int? NumberQuotaZ { get; set; }
        public Guid? AdmissionVolumeGuid { get; set; }

        public virtual AdmissionItemType AdmissionItemType { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual ICollection<DistributedAdmissionVolume> DistributedAdmissionVolume { get; set; }
    }
}
