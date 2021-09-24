using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicProfile
    {
        public OlympicProfile()
        {
            BenefitItemColympicTypeProfile = new HashSet<BenefitItemColympicTypeProfile>();
            BenefitItemCprofile = new HashSet<BenefitItemCprofile>();
            OlympicTypeProfile = new HashSet<OlympicTypeProfile>();
        }

        public int OlympicProfileId { get; set; }
        public string ProfileName { get; set; }

        public virtual ICollection<BenefitItemColympicTypeProfile> BenefitItemColympicTypeProfile { get; set; }
        public virtual ICollection<BenefitItemCprofile> BenefitItemCprofile { get; set; }
        public virtual ICollection<OlympicTypeProfile> OlympicTypeProfile { get; set; }
    }
}
