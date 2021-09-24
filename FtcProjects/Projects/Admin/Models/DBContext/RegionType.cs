using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class RegionType
    {
        public RegionType()
        {
            Address = new HashSet<Address>();
            Entrant1 = new HashSet<Entrant1>();
            Institution = new HashSet<Institution>();
            InstitutionHistory = new HashSet<InstitutionHistory>();
            OlympicDiplomant = new HashSet<OlympicDiplomant>();
        }

        public int CountryId { get; set; }
        public string Name { get; set; }
        public string OkatoCode { get; set; }
        public DateTime? OkatoModified { get; set; }
        public int DisplayOrder { get; set; }
        public string EsrpCode { get; set; }
        public int RegionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual CountryType Country { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Entrant1> Entrant1 { get; set; }
        public virtual ICollection<Institution> Institution { get; set; }
        public virtual ICollection<InstitutionHistory> InstitutionHistory { get; set; }
        public virtual ICollection<OlympicDiplomant> OlympicDiplomant { get; set; }
    }
}
