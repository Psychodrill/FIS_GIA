using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CompetitiveGroup
    {
        public CompetitiveGroup()
        {
            Application = new HashSet<Application>();
            ApplicationCompetitiveGroupItem = new HashSet<ApplicationCompetitiveGroupItem>();
            ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
            BenefitItemC = new HashSet<BenefitItemC>();
            CompetitiveGroupItem = new HashSet<CompetitiveGroupItem>();
            CompetitiveGroupProgram = new HashSet<CompetitiveGroupProgram>();
            CompetitiveGroupTargetItem = new HashSet<CompetitiveGroupTargetItem>();
            CompetitiveGroupToProgram = new HashSet<CompetitiveGroupToProgram>();
            EntranceTestItemC = new HashSet<EntranceTestItemC>();
            Rating = new HashSet<Rating>();
        }

        public int CompetitiveGroupId { get; set; }
        public int InstitutionId { get; set; }
        public string Name { get; set; }
        public short Course { get; set; }
        public string Uid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CampaignId { get; set; }
        public Guid? CompetitiveGroupGuid { get; set; }
        public bool? IsFromKrym { get; set; }
        public bool? IsAdditional { get; set; }
        public short? EducationFormId { get; set; }
        public short? EducationSourceId { get; set; }
        public short? EducationLevelId { get; set; }
        public int? DirectionId { get; set; }
        public int? IdLevelBudget { get; set; }
        public int DirectionFilterType { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual AdmissionItemType EducationForm { get; set; }
        public virtual AdmissionItemType EducationLevel { get; set; }
        public virtual AdmissionItemType EducationSource { get; set; }
        public virtual LevelBudget IdLevelBudgetNavigation { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual ICollection<Application> Application { get; set; }
        public virtual ICollection<ApplicationCompetitiveGroupItem> ApplicationCompetitiveGroupItem { get; set; }
        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
        public virtual ICollection<BenefitItemC> BenefitItemC { get; set; }
        public virtual ICollection<CompetitiveGroupItem> CompetitiveGroupItem { get; set; }
        public virtual ICollection<CompetitiveGroupProgram> CompetitiveGroupProgram { get; set; }
        public virtual ICollection<CompetitiveGroupTargetItem> CompetitiveGroupTargetItem { get; set; }
        public virtual ICollection<CompetitiveGroupToProgram> CompetitiveGroupToProgram { get; set; }
        public virtual ICollection<EntranceTestItemC> EntranceTestItemC { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }
    }
}
