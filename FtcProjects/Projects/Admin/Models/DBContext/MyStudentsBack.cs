using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class MyStudentsBack
    {
        public Guid? Id { get; set; }
        public string Fio { get; set; }
        public string BDate { get; set; }
    }
}
