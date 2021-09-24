using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Address
    {
        public Address()
        {
            Entrant1FactAddress = new HashSet<Entrant1>();
            Entrant1RegistrationAddress = new HashSet<Entrant1>();
        }

        public int AddressId { get; set; }
        public int? CountryId { get; set; }
        public int? RegionId { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string BuildingPart { get; set; }
        public string Room { get; set; }
        public string Phone { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual CountryType Country { get; set; }
        public virtual RegionType Region { get; set; }
        public virtual ICollection<Entrant1> Entrant1FactAddress { get; set; }
        public virtual ICollection<Entrant1> Entrant1RegistrationAddress { get; set; }
    }
}
