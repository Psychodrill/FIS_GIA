using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class MapCompetitiveGroupItem
    {
        public int NewCompetitiveGroupItemId { get; set; }
        public int? OldCompetitiveGroupItemId { get; set; }
        public string NewUid { get; set; }
        public string OldUid { get; set; }
    }
}
