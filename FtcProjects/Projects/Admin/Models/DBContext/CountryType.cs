using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CountryType
    {
        public CountryType()
        {
            Address = new HashSet<Address>();
            CountryDocuments = new HashSet<CountryDocuments>();
            ForeignInstitutions = new HashSet<ForeignInstitutions>();
            RegionType = new HashSet<RegionType>();
            Violation = new HashSet<Violation>();
        }

        public int CountryId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool HasRegions { get; set; }
        public string DigitCode { get; set; }
        public string Alfa2Code { get; set; }
        public string Alfa3Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? HasAgreement { get; set; }
        public DateTime? Expire { get; set; }

        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<CountryDocuments> CountryDocuments { get; set; }
        public virtual ICollection<ForeignInstitutions> ForeignInstitutions { get; set; }
        public virtual ICollection<RegionType> RegionType { get; set; }
        public virtual ICollection<Violation> Violation { get; set; }
    }
}
