using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkBenefitItemC
    {
        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public Guid? ParentCompetitiveGroup { get; set; }
        public Guid? ParentEntranceTestItem { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public short OlympicDiplomTypeId { get; set; }
        public short OlympicLevelFlags { get; set; }
        public short BenefitId { get; set; }
        public bool IsForAllOlympic { get; set; }
        public bool IsProfileSubject { get; set; }
        public string Uid { get; set; }
        public int OlympicYear { get; set; }
        public int? EgeMinValue { get; set; }
        public short? ClassFlags { get; set; }
        public bool? IsCreative { get; set; }
        public bool? IsAthletic { get; set; }
    }
}
