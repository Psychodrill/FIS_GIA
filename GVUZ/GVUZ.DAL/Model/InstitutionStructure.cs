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
    
    public partial class InstitutionStructure
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InstitutionStructure()
        {
            this.InstitutionStructure1 = new HashSet<InstitutionStructure>();
        }
    
        public int InstitutionStructureID { get; set; }
        public int InstitutionItemID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public short Depth { get; set; }
        public string Lineage { get; set; }
        public string UID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual InstitutionItem InstitutionItem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstitutionStructure> InstitutionStructure1 { get; set; }
        public virtual InstitutionStructure InstitutionStructure2 { get; set; }
    }
}
