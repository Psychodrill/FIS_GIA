using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ParentDirectionTmp
    {
        public int ParentDirectionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string EiisId { get; set; }
        public int? EsrpId { get; set; }
    }
}
