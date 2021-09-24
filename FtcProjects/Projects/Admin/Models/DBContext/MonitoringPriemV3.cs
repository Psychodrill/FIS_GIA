using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class MonitoringPriemV3
    {
        public int? Id { get; set; }
        public int? EsrpProdid { get; set; }
        public string Guid { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
    }
}
