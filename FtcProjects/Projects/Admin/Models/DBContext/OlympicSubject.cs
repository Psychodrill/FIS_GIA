using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicSubject
    {
        public int OlympicSubjectId { get; set; }
        public int OlympicTypeProfileId { get; set; }
        public int SubjectId { get; set; }

        public virtual OlympicTypeProfile OlympicTypeProfile { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
