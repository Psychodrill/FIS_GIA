using GVUZ.DAL.Dapper.Model.Directions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.ParentDirections
{
    [Table("ParentDirection")]
    public partial class ParentDirection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParentDirection()
        {
            Directions = new HashSet<Direction>();
        }
        public int ParentDirectionID { get; set; }

        [StringLength(8)]
        public string Code { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Direction> Directions { get; set; }
    }
}
