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
    
    public partial class BenefitItemC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BenefitItemC()
        {
            this.BenefitItemCOlympicType = new HashSet<BenefitItemCOlympicType>();
            this.BenefitItemCProfile = new HashSet<BenefitItemCProfile>();
            this.BenefitItemSubject = new HashSet<BenefitItemSubject>();
        }
    
        public int BenefitItemID { get; set; }
        public Nullable<int> EntranceTestItemID { get; set; }
        public short OlympicDiplomTypeID { get; set; }
        public Nullable<short> OlympicLevelFlags { get; set; }
        public short BenefitID { get; set; }
        public bool IsForAllOlympic { get; set; }
        public int CompetitiveGroupID { get; set; }
        public string UID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int OlympicYear { get; set; }
        public Nullable<int> EgeMinValue { get; set; }
        public Nullable<System.Guid> BenefitItemGUID { get; set; }
        public Nullable<short> ClassFlags { get; set; }
    
        public virtual Benefit Benefit { get; set; }
        public virtual EntranceTestItemC EntranceTestItemC { get; set; }
        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual OlympicDiplomType OlympicDiplomType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BenefitItemCOlympicType> BenefitItemCOlympicType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BenefitItemCProfile> BenefitItemCProfile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BenefitItemSubject> BenefitItemSubject { get; set; }
    }
}
