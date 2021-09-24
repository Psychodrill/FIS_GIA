using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Campaign
    {
        public Campaign()
        {
            AdmissionVolume = new HashSet<AdmissionVolume>();
            CampaignEducationLevel = new HashSet<CampaignEducationLevel>();
            CompetitiveGroup = new HashSet<CompetitiveGroup>();
            InstitutionAchievements = new HashSet<InstitutionAchievements>();
            OrderOfAdmission = new HashSet<OrderOfAdmission>();
            PlanAdmissionVolume = new HashSet<PlanAdmissionVolume>();
            Violation = new HashSet<Violation>();
            ViolationApplicationReception = new HashSet<ViolationApplicationReception>();
        }

        public int CampaignId { get; set; }
        public int InstitutionId { get; set; }
        public string Name { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public int EducationFormFlag { get; set; }
        public int StatusId { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CampaignGuid { get; set; }
        public short CampaignTypeId { get; set; }
        public int? CampaignAdmissionStatusId { get; set; }

        public virtual CampaignAdmissionStatus CampaignAdmissionStatus { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual ICollection<AdmissionVolume> AdmissionVolume { get; set; }
        public virtual ICollection<CampaignEducationLevel> CampaignEducationLevel { get; set; }
        public virtual ICollection<CompetitiveGroup> CompetitiveGroup { get; set; }
        public virtual ICollection<InstitutionAchievements> InstitutionAchievements { get; set; }
        public virtual ICollection<OrderOfAdmission> OrderOfAdmission { get; set; }
        public virtual ICollection<PlanAdmissionVolume> PlanAdmissionVolume { get; set; }
        public virtual ICollection<Violation> Violation { get; set; }
        public virtual ICollection<ViolationApplicationReception> ViolationApplicationReception { get; set; }
    }
}
