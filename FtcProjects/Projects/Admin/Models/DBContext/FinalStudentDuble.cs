using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class FinalStudentDuble
    {
        public string Fio { get; set; }
        public long? MyId { get; set; }
        public DateTime? Bday { get; set; }
        public string Post { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Name { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public int YearStart { get; set; }
        public string FullName { get; set; }
        public string УровеньОбразования { get; set; }
        public short? EducationLevelId { get; set; }
        public string NewCode { get; set; }
        public string DirectionName { get; set; }
        public string ИсточникФинансирования { get; set; }
        public string ФормаОбучения { get; set; }
        public string СтатусЗаявления { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int? PersonId { get; set; }
        public int EntrantId { get; set; }
        public int ApplicationId { get; set; }
    }
}
