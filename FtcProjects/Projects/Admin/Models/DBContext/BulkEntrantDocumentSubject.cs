using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkEntrantDocumentSubject
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
        public int? SubjectId { get; set; }
        public int? Value { get; set; }
    }
}
