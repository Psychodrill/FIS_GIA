using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Log
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set; }
    }
}
