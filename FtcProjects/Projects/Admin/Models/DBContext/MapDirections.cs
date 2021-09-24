using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class MapDirections
    {
        public int NewDirectionId { get; set; }
        public int? OldDirectionId { get; set; }
        public string NewUid { get; set; }
        public string OldUid { get; set; }
    }
}
