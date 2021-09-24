using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ViolationApplicationReception
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public DateTime? ViolationCheckDate { get; set; }
        public bool Has3DayViolationInforamtionApplicants { get; set; }
        public DateTime? DateSubmissionInforamtionApplicants { get; set; }
        public bool Has3DayViolationEntranceTests { get; set; }
        public DateTime? DateApprovalEntranceTests { get; set; }
        public bool Has3DayViolationReturnDocuments { get; set; }
        public DateTime? DateReturnDocuments { get; set; }
        public bool Has3DayViolationEnrollment { get; set; }
        public DateTime? DateEnrollmentOrder { get; set; }
        public bool Has3DayViolationExclusion { get; set; }
        public DateTime? DateExclusionOrder { get; set; }
        public bool? Has3DayViolationAchievements { get; set; }
        public DateTime? DateAchievements { get; set; }
        public int? CampaignId { get; set; }

        public virtual Application Application { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}
