using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dapper.Model.Campaigns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.Institutions
{
    [Table("Institution")]
    public partial class Institution
    {
        public Institution()
        {
            AllowedDirections = new HashSet<AllowedDirection>();
        }
        public int InstitutionID { get; set; }

        public short InstitutionTypeID { get; set; }

        [StringLength(1000)]
        public string FullName { get; set; }

        [StringLength(500)]
        public string BriefName { get; set; }

        public int? FormOfLawID { get; set; }

        public int? RegionID { get; set; }

        [StringLength(255)]
        public string Site { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        public bool HasMilitaryDepartment { get; set; }

        public bool HasHostel { get; set; }

        public int? HostelCapacity { get; set; }

        public bool HasHostelForEntrants { get; set; }

        public int? HostelAttachmentID { get; set; }

        [StringLength(10)]
        public string INN { get; set; }

        [Required]
        [StringLength(13)]
        public string OGRN { get; set; }

        public DateTime? AdmissionStructurePublishDate { get; set; }

        public DateTime? ReceivingApplicationDate { get; set; }

        public DateTime? DateUpdated { get; set; }

        public int? EsrpOrgID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(255)]
        public string OwnerDepartment { get; set; }

        public int? MainEsrpOrgId { get; set; }

        public int? FounderEsrpOrgId { get; set; }

        public int? StatusId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AllowedDirection> AllowedDirections { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}
