using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class PlanAdmissionVolume
    {
        public PlanAdmissionVolume()
        {
            DistributedPlanAdmissionVolume = new HashSet<DistributedPlanAdmissionVolume>();
        }

        public int PlanAdmissionVolumeId { get; set; }
        public int CampaignId { get; set; }
        public short AdmissionItemTypeId { get; set; }
        public int DirectionId { get; set; }
        public short EducationSourceId { get; set; }
        public short EducationFormId { get; set; }
        public int Number { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? AdmissionVolumeGuid { get; set; }

        public virtual AdmissionItemType AdmissionItemType { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual AdmissionItemType EducationForm { get; set; }
        public virtual AdmissionItemType EducationSource { get; set; }
        public virtual ICollection<DistributedPlanAdmissionVolume> DistributedPlanAdmissionVolume { get; set; }
    }
}
