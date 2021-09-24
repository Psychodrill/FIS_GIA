using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class IndividualAchivement
    {
        public int Iaid { get; set; }
        public int ApplicationId { get; set; }
        public string Iauid { get; set; }
        public string Ianame { get; set; }
        public decimal? Iamark { get; set; }
        public int? EntrantDocumentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? IdAchievement { get; set; }
        public bool? IsAdvantageRight { get; set; }

        public virtual Application Application { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual InstitutionAchievements IdAchievementNavigation { get; set; }
    }
}
