//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GVUZ.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AdmissionVolume
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdmissionVolume()
        {
            this.DistributedAdmissionVolume = new HashSet<DistributedAdmissionVolume>();
        }
    
        public int AdmissionVolumeID { get; set; }
        public int InstitutionID { get; set; }
        public short AdmissionItemTypeID { get; set; }
        public int? DirectionID { get; set; }
        public int NumberBudgetO { get; set; }
        public int NumberBudgetOZ { get; set; }
        public int NumberBudgetZ { get; set; }
        public int NumberPaidO { get; set; }
        public int NumberPaidOZ { get; set; }
        public int NumberPaidZ { get; set; }
        public string UID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> Course { get; set; }
        public Nullable<int> CampaignID { get; set; }
        public int NumberTargetO { get; set; }
        public int NumberTargetOZ { get; set; }
        public int NumberTargetZ { get; set; }
        public Nullable<int> NumberQuotaO { get; set; }
        public Nullable<int> NumberQuotaOZ { get; set; }
        public Nullable<int> NumberQuotaZ { get; set; }
        public Nullable<System.Guid> AdmissionVolumeGUID { get; set; }
        public Nullable<int> ParentDirectionID { get; set; }
    
        public virtual AdmissionItemType AdmissionItemType { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual Institution Institution { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DistributedAdmissionVolume> DistributedAdmissionVolume { get; set; }
    }
}
