using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GzguAetd
    {
        public int Idt { get; set; }
        public int ApplicationId { get; set; }
        public int? StatusId { get; set; }
        public int? ResultValue { get; set; }
        public int? BenefitId { get; set; }
        public int? SourceId { get; set; }
    }
}
