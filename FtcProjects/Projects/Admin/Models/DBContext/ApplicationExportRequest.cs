using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationExportRequest
    {
        public Guid RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestStatus { get; set; }
        public long InstitutionId { get; set; }
        public int YearStart { get; set; }
    }
}
