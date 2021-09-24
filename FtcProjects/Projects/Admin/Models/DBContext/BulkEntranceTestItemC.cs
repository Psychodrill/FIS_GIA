using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkEntranceTestItemC
    {
        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public Guid ParentId { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public short EntranceTestTypeId { get; set; }
        public string Form { get; set; }
        public decimal? MinScore { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Uid { get; set; }
        public int? EntranceTestPriority { get; set; }
        public string ReplacedEntranceTestItemUid { get; set; }
    }
}
