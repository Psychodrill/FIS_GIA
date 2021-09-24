//using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using GVUZ.DAL.Dapper.Model.Dictionary;
using GVUZ.DAL.Dapper.Model.Directions;
using GVUZ.DAL.Dapper.Model.Institutions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.AllowedDirections
{
    [Table("AllowedDirection")]
    public partial class AllowedDirection
    {
        public AllowedDirection()
        {
            AdmissionItemType = new AdmissionItemType();
            Direction = new Direction();
            Institution = new Institution();
        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InstitutionID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DirectionID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short AdmissionItemTypeID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual AdmissionItemType AdmissionItemType { get; set; }

        public virtual Direction Direction { get; set; }

        public virtual Institution Institution { get; set; }
    }
}
