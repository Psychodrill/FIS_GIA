using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkApplicationSelectedCompetitiveGroupItem
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public int CompetitiveGroupItemId { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
