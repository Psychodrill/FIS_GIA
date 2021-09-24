using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CampaignTypes
    {
        public CampaignTypes()
        {
            EduLevelsToCampaignTypes = new HashSet<EduLevelsToCampaignTypes>();
        }

        public short CampaignTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EduLevelsToCampaignTypes> EduLevelsToCampaignTypes { get; set; }
    }
}
