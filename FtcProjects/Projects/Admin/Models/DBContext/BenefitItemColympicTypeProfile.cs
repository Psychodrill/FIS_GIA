using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BenefitItemColympicTypeProfile
    {
        public int BenefitItemColympicTypeProfileId { get; set; }
        public int BenefitItemColympicTypeId { get; set; }
        public int OlympicProfileId { get; set; }

        public virtual BenefitItemColympicType BenefitItemColympicType { get; set; }
        public virtual OlympicProfile OlympicProfile { get; set; }
    }
}
