using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EduLevelsToCampaignTypes
    {
        public short CampaignTypeId { get; set; }
        public short AdmissionItemTypeId { get; set; }

        public virtual AdmissionItemType AdmissionItemType { get; set; }
        public virtual CampaignTypes CampaignType { get; set; }
    }
}
