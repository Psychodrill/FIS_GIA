using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkInstitutionProgram
    {
        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public Guid ParentId { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
