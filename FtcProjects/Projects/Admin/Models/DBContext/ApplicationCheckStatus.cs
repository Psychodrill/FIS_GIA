using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationCheckStatus
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
