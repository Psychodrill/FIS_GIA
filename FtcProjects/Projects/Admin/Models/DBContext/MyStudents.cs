using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class MyStudents
    {
        public Guid? Id { get; set; }
        public string Fio { get; set; }
        public string BDate { get; set; }
        public DateTime? BDateFis { get; set; }
    }
}
