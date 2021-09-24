using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class FinalTable
    {
        public string Fio { get; set; }
        public string BDate { get; set; }
        public string FullName { get; set; }
        public string OchnayaBakSpec { get; set; }
        public string BudgContr { get; set; }
        public string Name { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public int YearStart { get; set; }
        public Guid? MyStudentsFk { get; set; }
    }
}
