using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Student1
    {
        public string Fio { get; set; }
        public DateTime? Bday { get; set; }
        public string BdayInt { get; set; }
        public string Post { get; set; }
        public long? Id { get; set; }
    }
}
