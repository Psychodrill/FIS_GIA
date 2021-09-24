using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CampaignOrderDateCatalog
    {
        public int YearStart { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime TargetOrderDate { get; set; }
        public DateTime Stage1OrderDate { get; set; }
        public DateTime Stage2OrderDate { get; set; }
        public DateTime PaidOrderDate { get; set; }
        public int? PreviousUseDepth { get; set; }
    }
}
