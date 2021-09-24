using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentIdentityTmp
    {
        public int EntrantDocumentId { get; set; }
        public int? IdentityDocumentTypeId { get; set; }
        public int? GenderTypeId { get; set; }
        public int? NationalityTypeId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string SubdivisionCode { get; set; }
    }
}
