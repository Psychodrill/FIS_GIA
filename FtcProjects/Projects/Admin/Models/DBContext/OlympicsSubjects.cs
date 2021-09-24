using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicsSubjects
    {
        public int? IdSubjectFbs { get; set; }
        public int? PersonId { get; set; }
        public int? OlympicNumber { get; set; }
        public string OtName { get; set; }
        public string Place { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string ProfileName { get; set; }
        public string BirthDate { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string SName { get; set; }
    }
}
