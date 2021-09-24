using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentInternationalOlympic
    {
        public int EntrantDocumentId { get; set; }
        public int CountryId { get; set; }
        public string Profile { get; set; }
        public string OlympicPlace { get; set; }
        public DateTime? OlympicDate { get; set; }

        public virtual CountryType Country { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
    }
}
