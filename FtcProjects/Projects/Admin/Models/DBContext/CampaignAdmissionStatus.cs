using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CampaignAdmissionStatus
    {
        public CampaignAdmissionStatus()
        {
            Campaign = new HashSet<Campaign>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Campaign> Campaign { get; set; }
    }
}
