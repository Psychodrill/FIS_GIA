using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class VwAspnetProfiles
    {
        public Guid UserId { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int? DataSize { get; set; }
    }
}
