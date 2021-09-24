using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GetInstitutionAchievements
    {
        public int? CampaignId { get; set; }
        public int InstitutionId { get; set; }
        public int IdAchievement { get; set; }
        public string Uid { get; set; }
        public string CategoryName { get; set; }
        public string AchivementName { get; set; }
        public decimal MaxValue { get; set; }
    }
}
