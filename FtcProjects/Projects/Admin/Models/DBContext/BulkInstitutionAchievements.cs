using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkInstitutionAchievements
    {
        public int? Id { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public int IdCategory { get; set; }
        public decimal MaxValue { get; set; }
        public int? CampaignId { get; set; }
    }
}
