using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BlkApplicationSelectedCompetitiveGroupItem
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
