using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkCampaign
    {
        public Guid Id { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public int EducationFormFlag { get; set; }
        public int StatusId { get; set; }
        public short CampaignTypeId { get; set; }
    }
}
