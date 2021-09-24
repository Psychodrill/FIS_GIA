using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BenefitItemCprofile
    {
        public int BenefitItemCprofileId { get; set; }
        public int BenefitItemId { get; set; }
        public int OlympicProfileId { get; set; }

        public virtual BenefitItemC BenefitItem { get; set; }
        public virtual OlympicProfile OlympicProfile { get; set; }
    }
}
