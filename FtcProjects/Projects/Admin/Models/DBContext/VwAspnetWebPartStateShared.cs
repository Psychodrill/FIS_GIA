using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class VwAspnetWebPartStateShared
    {
        public Guid PathId { get; set; }
        public int? DataSize { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
