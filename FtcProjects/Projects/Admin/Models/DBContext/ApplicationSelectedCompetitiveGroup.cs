using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationSelectedCompetitiveGroup
    {
        public int ItemId { get; set; }
        public int ApplicationId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public short? CalculatedBenefitId { get; set; }
        public decimal? CalculatedRating { get; set; }
    }
}
