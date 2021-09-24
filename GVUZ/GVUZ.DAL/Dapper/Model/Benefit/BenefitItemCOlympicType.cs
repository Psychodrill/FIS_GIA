using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.Model.Benefit
{
    [Table("BenefitItemCOlympicType")]
    public partial class BenefitItemCOlympicType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BenefitItemCOlympicType()
        {
            //DistributedAdmissionVolumes = new HashSet<DistributedAdmissionVolume>();
        }

        public int ID { get; set; }
        public int BenefitItemID { get; set; }
        public int OlympicTypeID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short OlympicLevel { get; set; }
        public short ClassFlags { get; set; }
        public short OlympicLevelFlags { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<DistributedAdmissionVolume> DistributedAdmissionVolumes { get; set; }

        public virtual BenefitItemC BenefitItemC { get; set; }
    }
}
