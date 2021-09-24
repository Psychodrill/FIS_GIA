using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class MonitoringPriem
    {
        public int? Id { get; set; }
        public int? EsrpProdId { get; set; }
        public string Guid { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Action { get; set; }
    }
}
