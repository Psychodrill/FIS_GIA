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
    using System.ComponentModel;
    public partial class OlympicProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OlympicProfile()
        {
            this.BenefitItemCOlympicTypeProfile = new HashSet<BenefitItemCOlympicTypeProfile>();
            this.BenefitItemCProfile = new HashSet<BenefitItemCProfile>();
            this.EntrantDocumentOlympic = new HashSet<EntrantDocumentOlympic>();
            this.OlympicTypeProfile = new HashSet<OlympicTypeProfile>();
        }
    
        public int OlympicProfileID { get; set; }

        [DisplayName("������� ���������")]
        public string ProfileName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BenefitItemCOlympicTypeProfile> BenefitItemCOlympicTypeProfile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BenefitItemCProfile> BenefitItemCProfile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntrantDocumentOlympic> EntrantDocumentOlympic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OlympicTypeProfile> OlympicTypeProfile { get; set; }
    }
}
