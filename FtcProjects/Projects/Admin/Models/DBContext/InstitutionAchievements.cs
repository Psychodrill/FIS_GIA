using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionAchievements
    {
        public InstitutionAchievements()
        {
            IndividualAchivement = new HashSet<IndividualAchivement>();
        }

        public int IdAchievement { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public int IdCategory { get; set; }
        public decimal MaxValue { get; set; }
        public int? CampaignId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual IndividualAchievementsCategory IdCategoryNavigation { get; set; }
        public virtual ICollection<IndividualAchivement> IndividualAchivement { get; set; }
    }
}
