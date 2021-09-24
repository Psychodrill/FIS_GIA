using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TmpPartId
    {
        public Guid PartId { get; set; }
        public int? Year { get; set; }
    }
}
