using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AbitInstitutionIndAch
    {
        public int InstitutionId { get; set; }
        public int YearStart { get; set; }
        public string Name { get; set; }
        public decimal? MaxValue { get; set; }
        public string Level { get; set; }
        public short LevelId { get; set; }
    }
}
