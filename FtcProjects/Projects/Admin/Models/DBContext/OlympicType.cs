using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicType
    {
        public OlympicType()
        {
            BenefitItemColympicType = new HashSet<BenefitItemColympicType>();
            OlympicTypeProfile = new HashSet<OlympicTypeProfile>();
        }

        public int OlympicId { get; set; }
        public string Name { get; set; }
        public int? OlympicNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int OlympicYear { get; set; }

        public virtual ICollection<BenefitItemColympicType> BenefitItemColympicType { get; set; }
        public virtual ICollection<OlympicTypeProfile> OlympicTypeProfile { get; set; }
    }
}
