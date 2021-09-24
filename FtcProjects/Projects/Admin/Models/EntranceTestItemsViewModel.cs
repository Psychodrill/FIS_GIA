using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class EntranceTestItemsViewModel
    {
        public int CompetitiveGroupId { get; set; }
        public short EntranceTestTypeId { get; set; }
        public int EntranceTestItemId { get; set; }
        public int? ReplacedEntranceTestItemId { get; set; }
        public decimal? MinScore { get; set; }
        public int? EntranceTestPriority { get; set; }
        public string Name { get; set; }
    }
}
