using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AdmissionItemType
    {
        public AdmissionItemType()
        {
            AdmissionVolume = new HashSet<AdmissionVolume>();
            AllowedDirections = new HashSet<AllowedDirections>();
            ApplicationConsidered = new HashSet<ApplicationConsidered>();
            ApplicationOrderEducationForm = new HashSet<Application>();
            ApplicationOrderEducationSource = new HashSet<Application>();
            CampaignEducationLevel = new HashSet<CampaignEducationLevel>();
            CompetitiveGroupEducationForm = new HashSet<CompetitiveGroup>();
            CompetitiveGroupEducationLevel = new HashSet<CompetitiveGroup>();
            CompetitiveGroupEducationSource = new HashSet<CompetitiveGroup>();
            EduLevels = new HashSet<EduLevels>();
            EduLevelsToCampaignTypes = new HashSet<EduLevelsToCampaignTypes>();
            EduLevelsToUgsCode = new HashSet<EduLevelsToUgsCode>();
            LicensedDirection = new HashSet<LicensedDirection>();
            OrderOfAdmissionEducationForm = new HashSet<OrderOfAdmission>();
            OrderOfAdmissionEducationLevel = new HashSet<OrderOfAdmission>();
            OrderOfAdmissionEducationSource = new HashSet<OrderOfAdmission>();
            PlanAdmissionVolumeAdmissionItemType = new HashSet<PlanAdmissionVolume>();
            PlanAdmissionVolumeEducationForm = new HashSet<PlanAdmissionVolume>();
            PlanAdmissionVolumeEducationSource = new HashSet<PlanAdmissionVolume>();
            ViolationEducationForm = new HashSet<Violation>();
            ViolationEducationLevel = new HashSet<Violation>();
        }

        public short ItemTypeId { get; set; }
        public string Name { get; set; }
        public short ItemLevel { get; set; }
        public bool CanBeSkipped { get; set; }
        public bool AutoCopyName { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<AdmissionVolume> AdmissionVolume { get; set; }
        public virtual ICollection<AllowedDirections> AllowedDirections { get; set; }
        public virtual ICollection<ApplicationConsidered> ApplicationConsidered { get; set; }
        public virtual ICollection<Application> ApplicationOrderEducationForm { get; set; }
        public virtual ICollection<Application> ApplicationOrderEducationSource { get; set; }
        public virtual ICollection<CampaignEducationLevel> CampaignEducationLevel { get; set; }
        public virtual ICollection<CompetitiveGroup> CompetitiveGroupEducationForm { get; set; }
        public virtual ICollection<CompetitiveGroup> CompetitiveGroupEducationLevel { get; set; }
        public virtual ICollection<CompetitiveGroup> CompetitiveGroupEducationSource { get; set; }
        public virtual ICollection<EduLevels> EduLevels { get; set; }
        public virtual ICollection<EduLevelsToCampaignTypes> EduLevelsToCampaignTypes { get; set; }
        public virtual ICollection<EduLevelsToUgsCode> EduLevelsToUgsCode { get; set; }
        public virtual ICollection<LicensedDirection> LicensedDirection { get; set; }
        public virtual ICollection<OrderOfAdmission> OrderOfAdmissionEducationForm { get; set; }
        public virtual ICollection<OrderOfAdmission> OrderOfAdmissionEducationLevel { get; set; }
        public virtual ICollection<OrderOfAdmission> OrderOfAdmissionEducationSource { get; set; }
        public virtual ICollection<PlanAdmissionVolume> PlanAdmissionVolumeAdmissionItemType { get; set; }
        public virtual ICollection<PlanAdmissionVolume> PlanAdmissionVolumeEducationForm { get; set; }
        public virtual ICollection<PlanAdmissionVolume> PlanAdmissionVolumeEducationSource { get; set; }
        public virtual ICollection<Violation> ViolationEducationForm { get; set; }
        public virtual ICollection<Violation> ViolationEducationLevel { get; set; }
    }
}
