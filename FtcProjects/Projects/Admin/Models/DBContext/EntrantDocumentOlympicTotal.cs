using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentOlympicTotal
    {
        public int EntrantDocumentId { get; set; }
        public int? DiplomaTypeId { get; set; }
        public string OlympicPlace { get; set; }
        public DateTime? OlympicDate { get; set; }

        public virtual EntrantDocument EntrantDocument { get; set; }
    }
}
