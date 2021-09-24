using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntrantDocumentOlympic
    {
        public int EntrantDocumentId { get; set; }
        public int? DiplomaTypeId { get; set; }
        public int? OlympicId { get; set; }
        public int? ClassNumber { get; set; }
        public int? OlympicTypeProfileId { get; set; }
        public int? ProfileSubjectId { get; set; }
        public int? EgeSubjectId { get; set; }

        public virtual Subject EgeSubject { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual OlympicTypeProfile OlympicTypeProfile { get; set; }
        public virtual Subject ProfileSubject { get; set; }
    }
}
