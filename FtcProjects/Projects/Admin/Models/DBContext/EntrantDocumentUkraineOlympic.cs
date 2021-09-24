using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentUkraineOlympic
    {
        public int EntrantDocumentId { get; set; }
        public short DiplomaTypeId { get; set; }
        public string Profile { get; set; }
        public string OlympicPlace { get; set; }
        public DateTime? OlympicDate { get; set; }

        public virtual OlympicDiplomType DiplomaType { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
    }
}
