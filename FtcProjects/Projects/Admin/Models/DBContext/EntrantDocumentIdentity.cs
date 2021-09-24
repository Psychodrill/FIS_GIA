using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentIdentity
    {
        public int EntrantDocumentId { get; set; }
        public int? IdentityDocumentTypeId { get; set; }
        public int? GenderTypeId { get; set; }
        public int? NationalityTypeId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string SubdivisionCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual IdentityDocumentType IdentityDocumentType { get; set; }
        public virtual CountryType NationalityType { get; set; }
    }
}
