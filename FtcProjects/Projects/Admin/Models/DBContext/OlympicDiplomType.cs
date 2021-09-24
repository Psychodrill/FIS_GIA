using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicDiplomType
    {
        public OlympicDiplomType()
        {
            BenefitItemC = new HashSet<BenefitItemC>();
            OlympicDiplomant = new HashSet<OlympicDiplomant>();
        }

        public short OlympicDiplomTypeId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<BenefitItemC> BenefitItemC { get; set; }
        public virtual ICollection<OlympicDiplomant> OlympicDiplomant { get; set; }
    }
}
