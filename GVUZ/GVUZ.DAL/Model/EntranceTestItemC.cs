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
    
    public partial class EntranceTestItemC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EntranceTestItemC()
        {
            this.ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
            this.BenefitItemC = new HashSet<BenefitItemC>();
            this.EntranceTestItemC1 = new HashSet<EntranceTestItemC>();
        }
    
        public int EntranceTestItemID { get; set; }
        public int CompetitiveGroupID { get; set; }
        public short EntranceTestTypeID { get; set; }
        public Nullable<decimal> MinScore { get; set; }
        public Nullable<int> SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string UID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> EntranceTestPriority { get; set; }
        public Nullable<System.Guid> EntranceTestItemGUID { get; set; }
        public Nullable<bool> IsForSPOandVO { get; set; }
        public Nullable<int> ReplacedEntranceTestItemID { get; set; }
        public bool IsFirst { get; set; }
        public bool IsSecond { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BenefitItemC> BenefitItemC { get; set; }
        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntranceTestItemC> EntranceTestItemC1 { get; set; }
        public virtual EntranceTestItemC EntranceTestItemC2 { get; set; }
        public virtual EntranceTestType EntranceTestType { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
