using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkCompetitiveGroup
    {
        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public string Name { get; set; }
        public short Course { get; set; }
        public string Uid { get; set; }
        public int? CampaignId { get; set; }
        public int DirectionId { get; set; }
        public int EducationFormId { get; set; }
        public int EducationLevelId { get; set; }
        public int EducationSourceId { get; set; }
        public bool IsFromKrym { get; set; }
        public bool IsAdditional { get; set; }
        public int? IdLevelBudget { get; set; }
    }
}
