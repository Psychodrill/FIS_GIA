using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Benefitbudg
    {
        public int InstitutionId { get; set; }
        public string Vuz { get; set; }
        public string Nps { get; set; }
        public double? MinEntranceValue { get; set; }
        public int? PlaceCount { get; set; }
        public int? EntrantCount { get; set; }
        public double? AvgEgeClear { get; set; }
        public double? AvgEgeBvi { get; set; }
        public int? InOrderBviVsosh { get; set; }
        public int? InOrderBviOsh { get; set; }
        public int? InOrderMaxegevsosh { get; set; }
        public int? InOrderMaxegeosh { get; set; }
    }
}
