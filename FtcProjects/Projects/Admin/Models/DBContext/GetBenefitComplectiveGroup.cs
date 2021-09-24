using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GetBenefitComplectiveGroup
    {
        public int BenefitItemId { get; set; }
        public Guid? BenefitItemGuid { get; set; }
        public int CompetitiveGroupId { get; set; }
        public int? EntranceTestItemId { get; set; }
        public bool IsForAllOlympic { get; set; }
        public short BenefitId { get; set; }
        public string BenefitName { get; set; }
        public string BenefitShortName { get; set; }
        public int? EgeMinValue { get; set; }
        public short OlympicDiplomTypeId { get; set; }
        public string OlympicDiplomTypeName { get; set; }
        public int OlympicYear { get; set; }
    }
}
