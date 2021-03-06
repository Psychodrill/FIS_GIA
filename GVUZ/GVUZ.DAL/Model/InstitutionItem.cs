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
    
    public partial class InstitutionItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InstitutionItem()
        {
            this.InstitutionItem1 = new HashSet<InstitutionItem>();
            this.InstitutionStructure = new HashSet<InstitutionStructure>();
        }
    
        public int InstitutionItemID { get; set; }
        public int InstitutionID { get; set; }
        public short ItemTypeID { get; set; }
        public string Name { get; set; }
        public string BriefName { get; set; }
        public Nullable<int> DirectionID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Site { get; set; }
        public string UID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual Direction Direction { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual InstitutionItemType InstitutionItemType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstitutionItem> InstitutionItem1 { get; set; }
        public virtual InstitutionItem InstitutionItem2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstitutionStructure> InstitutionStructure { get; set; }
    }
}
