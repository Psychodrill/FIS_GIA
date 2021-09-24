using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkCompetitiveGroupProgram
    {
        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public Guid ParentId { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionProgramId { get; set; }
        public string Uid { get; set; }
    }
}
