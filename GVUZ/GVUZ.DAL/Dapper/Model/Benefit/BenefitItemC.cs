using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.Model.Benefit
{
    [Table("BenefitItemC")]
    public partial class BenefitItemC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BenefitItemC()
        {
            BenefitItemCOlympicTypes = new HashSet<BenefitItemCOlympicType>();
        }

        public int BenefitItemID { get; set; }
        public int? EntranceTestItemID { get; set; }

        public short OlympicDiplomTypeID { get; set; }
        public short? OlympicLevelFlags { get; set; }
        public short BenefitID { get; set; }
        public bool IsForAllOlympic { get; set; }
        public int CompetitiveGroupID { get; set; }
        public string UID { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int OlympicYear { get; set; }
        public int? EgeMinValue { get; set; }
        public Guid? BenefitItemGUID { get; set; }
        public short? ClassFlags { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BenefitItemCOlympicType> BenefitItemCOlympicTypes { get; set; }
    }
}
