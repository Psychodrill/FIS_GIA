using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BenefitItemColympicType
    {
        public BenefitItemColympicType()
        {
            BenefitItemColympicTypeProfile = new HashSet<BenefitItemColympicTypeProfile>();
        }

        public int Id { get; set; }
        public int BenefitItemId { get; set; }
        public int OlympicTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short? ClassFlags { get; set; }
        public Guid? Guid { get; set; }
        public short? OlympicLevelFlags { get; set; }

        public virtual BenefitItemC BenefitItem { get; set; }
        public virtual OlympicType OlympicType { get; set; }
        public virtual ICollection<BenefitItemColympicTypeProfile> BenefitItemColympicTypeProfile { get; set; }
    }
}
