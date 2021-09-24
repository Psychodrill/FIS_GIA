using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dapper.Model.CompetitiveGroups;
using GVUZ.DAL.Dapper.Model.ParentDirections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.Directions
{
    [Table("Direction")]
    public partial class Direction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Direction()
        {
            AllowedDirections = new HashSet<AllowedDirection>();
            ParentDirection = new ParentDirection();
        }
        public int DirectionID { get; set; }

        [StringLength(6)]
        public string Code { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public int? ParentID { get; set; }

        [StringLength(50)]
        public string SYS_GUID { get; set; }

        [StringLength(500)]
        public string EDULEVEL { get; set; }

        [StringLength(500)]
        public string EDUPROGRAMTYPE { get; set; }

        [StringLength(8)]
        public string UGSCODE { get; set; }

        [StringLength(500)]
        public string UGSNAME { get; set; }

        [StringLength(10)]
        public string QUALIFICATIONCODE { get; set; }

        [StringLength(500)]
        public string QUALIFICATIONNAME { get; set; }

        [StringLength(50)]
        public string PERIOD { get; set; }

        [StringLength(500)]
        public string EDU_DIRECTORY { get; set; }

        [StringLength(500)]
        public string EDUPR_ADDITIONAL { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(8)]
        public string NewCode { get; set; }

        public short? EducationLevelId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdmissionVolume> AdmissionVolumes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AllowedDirection> AllowedDirections { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ApplicationConsidered> ApplicationConsidereds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompetitiveGroup> CompetitiveGroups { get; set; }

        public virtual ParentDirection ParentDirection { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<DirectionSubjectLinkDirection> DirectionSubjectLinkDirections { get; set; }

        //public virtual EntranceTestCreativeDirection EntranceTestCreativeDirection { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EntranceTestProfileDirection> EntranceTestProfileDirections { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<InstitutionItem> InstitutionItems { get; set; }
    }
}
