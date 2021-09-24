using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkCampaignDate
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int Course { get; set; }
        public short EducationLevelId { get; set; }
        public short EducationFormId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateOrder { get; set; }
        public string Uid { get; set; }
        public bool IsActive { get; set; }
        public int Stage { get; set; }
        public short EducationSourceId { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
