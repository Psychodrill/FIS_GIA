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
    
    public partial class RVIPersons
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RVIPersons()
        {
            this.Entrant = new HashSet<Entrant>();
            this.OlympicDiplomant = new HashSet<OlympicDiplomant>();
            this.RVIPersonIdentDocs = new HashSet<RVIPersonIdentDocs>();
        }
    
        public int PersonId { get; set; }
        public bool IsRecordDeleted { get; set; }
        public string NormSurname { get; set; }
        public string NormName { get; set; }
        public string NormSecondName { get; set; }
        public Nullable<System.DateTime> BirthDay { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string SNILS { get; set; }
        public string INN { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public System.DateTime IntegralUpdateDate { get; set; }
        public Nullable<System.Guid> ParticipantID { get; set; }
        public Nullable<int> UseYear { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Entrant> Entrant { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OlympicDiplomant> OlympicDiplomant { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RVIPersonIdentDocs> RVIPersonIdentDocs { get; set; }
    }
}
