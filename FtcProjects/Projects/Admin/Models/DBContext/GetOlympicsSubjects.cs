using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GetOlympicsSubjects
    {
        public int OlympicSubjectId { get; set; }
        public int OlympicTypeProfileId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
}
