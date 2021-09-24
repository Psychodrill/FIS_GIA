using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GetEntranceTestItemC
    {
        public int CompetitiveGroupId { get; set; }
        public int EntranceTestItemId { get; set; }
        public string Uid { get; set; }
        public Guid? Guid { get; set; }
        public decimal? MinScore { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectOlympName { get; set; }
        public string SubjectName { get; set; }
        public short EntranceTestTypeId { get; set; }
        public string EntranceTestTypeName { get; set; }
        public int? EntranceTestPriority { get; set; }
    }
}
