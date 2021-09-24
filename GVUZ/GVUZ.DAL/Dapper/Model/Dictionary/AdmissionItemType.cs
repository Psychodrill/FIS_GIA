using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dapper.Model.Campaigns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.Dictionary
{
    [Table("AdmissionItemType")]
    public partial class AdmissionItemType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdmissionItemType()
        {
            //AdmissionVolumes = new HashSet<AdmissionVolume>();
            AllowedDirections = new HashSet<AllowedDirection>();
            //Applications = new HashSet<Application>();
            //Applications1 = new HashSet<Application>();
            //ApplicationConsidereds = new HashSet<ApplicationConsidered>();
            CampaignEducationLevels = new HashSet<CampaignEducationLevel>();
            //CompetitiveGroups = new HashSet<CompetitiveGroup>();
            //CompetitiveGroups1 = new HashSet<CompetitiveGroup>();
            //CompetitiveGroups2 = new HashSet<CompetitiveGroup>();
            //EduLevels = new HashSet<EduLevel>();
            //OrderOfAdmissions = new HashSet<OrderOfAdmission>();
            //OrderOfAdmissions1 = new HashSet<OrderOfAdmission>();
            //OrderOfAdmissions2 = new HashSet<OrderOfAdmission>();
            //CampaignTypes = new HashSet<CampaignType>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short ItemTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public short ItemLevel { get; set; }

        public bool CanBeSkipped { get; set; }

        public bool AutoCopyName { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<AdmissionVolume> AdmissionVolumes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AllowedDirection> AllowedDirections { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Application> Applications { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Application> Applications1 { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ApplicationConsidered> ApplicationConsidereds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CampaignEducationLevel> CampaignEducationLevels { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CompetitiveGroup> CompetitiveGroups { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CompetitiveGroup> CompetitiveGroups1 { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CompetitiveGroup> CompetitiveGroups2 { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EduLevel> EduLevels { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<OrderOfAdmission> OrderOfAdmissions { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<OrderOfAdmission> OrderOfAdmissions1 { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<OrderOfAdmission> OrderOfAdmissions2 { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<CampaignType> CampaignTypes { get; set; }
    }
}
