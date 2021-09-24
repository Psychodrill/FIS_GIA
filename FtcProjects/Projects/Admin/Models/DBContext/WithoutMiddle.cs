using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class WithoutMiddle
    {
        public int Id { get; set; }
        public string Fio { get; set; }
        public DateTime? BDate { get; set; }
    }
}
