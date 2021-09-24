using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class DbaIndexDefragLog
    {
        public int IndexDefragId { get; set; }
        public int ObjectId { get; set; }
        public string ObjectName { get; set; }
        public int IndexId { get; set; }
        public string IndexName { get; set; }
        public double Fragmentation { get; set; }
        public int PageCount { get; set; }
        public DateTime DateTimeStart { get; set; }
        public int DurationSeconds { get; set; }
    }
}
