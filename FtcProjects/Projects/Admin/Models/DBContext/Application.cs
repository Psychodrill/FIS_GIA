using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Application
    {
        public Application()
        {
            ApplicationCompetitiveGroupItem = new HashSet<ApplicationCompetitiveGroupItem>();
            ApplicationCompositionResults = new HashSet<ApplicationCompositionResults>();
            ApplicationCompositionResultsApprob = new HashSet<ApplicationCompositionResultsApprob>();
            ApplicationConsidered = new HashSet<ApplicationConsidered>();
            ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
            ApplicationEntrantDocument = new HashSet<ApplicationEntrantDocument>();
            ApplicationExtra = new HashSet<ApplicationExtra>();
            ApplicationForcedAdmissionDocument = new HashSet<ApplicationForcedAdmissionDocument>();
            IndividualAchivement = new HashSet<IndividualAchivement>();
            OrderOfAdmissionHistory = new HashSet<OrderOfAdmissionHistory>();
            RatingList = new HashSet<RatingList>();
            ViolationApplicationReception = new HashSet<ViolationApplicationReception>();
        }

        public int ApplicationId { get; set; }
        public int EntrantId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int InstitutionId { get; set; }
        public bool? ApproveInstitutionCount { get; set; }
        public bool? NeedHostel { get; set; }
        public bool? FirstHigherEducation { get; set; }
        public bool? ApprovePersonalData { get; set; }
        public bool? FamiliarWithLicenseAndRules { get; set; }
        public bool? FamiliarWithAdmissionType { get; set; }
        public bool? FamiliarWithOriginalDocumentDeliveryDate { get; set; }
        public int StatusId { get; set; }
        public int WizardStepId { get; set; }
        public int ViolationId { get; set; }
        public string StatusDecision { get; set; }
        public DateTime? LastCheckDate { get; set; }
        public string ViolationErrors { get; set; }
        public DateTime? PublishDate { get; set; }
        public byte SourceId { get; set; }
        public string ApplicationNumber { get; set; }
        public bool OriginalDocumentsReceived { get; set; }
        public int? OrderCompetitiveGroupId { get; set; }
        public int? OrderOfAdmissionId { get; set; }
        public decimal? OrderCalculatedRating { get; set; }
        public short? OrderCalculatedBenefitId { get; set; }
        public short? OrderEducationFormId { get; set; }
        public short? OrderEducationSourceId { get; set; }
        public DateTime? LastDenyDate { get; set; }
        public string Uid { get; set; }
        public bool IsRequiresBudgetO { get; set; }
        public bool IsRequiresBudgetOz { get; set; }
        public bool IsRequiresBudgetZ { get; set; }
        public bool IsRequiresPaidO { get; set; }
        public bool IsRequiresPaidOz { get; set; }
        public bool IsRequiresPaidZ { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? OriginalDocumentsReceivedDate { get; set; }
        public DateTime? LastEgeDocumentsCheckDate { get; set; }
        public int? OrderCompetitiveGroupTargetId { get; set; }
        public bool IsRequiresTargetO { get; set; }
        public bool IsRequiresTargetOz { get; set; }
        public bool IsRequiresTargetZ { get; set; }
        public Guid? ApplicationGuid { get; set; }
        public int? Priority { get; set; }
        public int? OrderIdLevelBudget { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsDistant { get; set; }
        public int? IsDisabledDocumentId { get; set; }
        public string DistantPlace { get; set; }
        public decimal? IndividualAchivementsMark { get; set; }
        public int? ApplicationForcedAdmissionReasonsId { get; set; }
        public int? ReturnDocumentsTypeId { get; set; }
        public DateTime? ReturnDocumentsDate { get; set; }
        public int? OrderCompetitiveGroupItemId { get; set; }

        public virtual ApplicationForcedAdmissionReason ApplicationForcedAdmissionReasons { get; set; }
        public virtual Entrant1 Entrant { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual EntrantDocument IsDisabledDocument { get; set; }
        public virtual Benefit OrderCalculatedBenefit { get; set; }
        public virtual CompetitiveGroup OrderCompetitiveGroup { get; set; }
        public virtual CompetitiveGroupTarget OrderCompetitiveGroupTarget { get; set; }
        public virtual AdmissionItemType OrderEducationForm { get; set; }
        public virtual AdmissionItemType OrderEducationSource { get; set; }
        public virtual LevelBudget OrderIdLevelBudgetNavigation { get; set; }
        public virtual OrderOfAdmission OrderOfAdmission { get; set; }
        public virtual ApplicationReturnDocumentsType ReturnDocumentsType { get; set; }
        public virtual ApplicationStatusType Status { get; set; }
        public virtual ViolationType Violation { get; set; }
        public virtual ICollection<ApplicationCompetitiveGroupItem> ApplicationCompetitiveGroupItem { get; set; }
        public virtual ICollection<ApplicationCompositionResults> ApplicationCompositionResults { get; set; }
        public virtual ICollection<ApplicationCompositionResultsApprob> ApplicationCompositionResultsApprob { get; set; }
        public virtual ICollection<ApplicationConsidered> ApplicationConsidered { get; set; }
        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
        public virtual ICollection<ApplicationEntrantDocument> ApplicationEntrantDocument { get; set; }
        public virtual ICollection<ApplicationExtra> ApplicationExtra { get; set; }
        public virtual ICollection<ApplicationForcedAdmissionDocument> ApplicationForcedAdmissionDocument { get; set; }
        public virtual ICollection<IndividualAchivement> IndividualAchivement { get; set; }
        public virtual ICollection<OrderOfAdmissionHistory> OrderOfAdmissionHistory { get; set; }
        public virtual ICollection<RatingList> RatingList { get; set; }
        public virtual ICollection<ViolationApplicationReception> ViolationApplicationReception { get; set; }
    }
}
