using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OrderOfAdmissionHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ApplicationId { get; set; }
        public DateTime? DatePublished { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Application Application { get; set; }
        public virtual OrderOfAdmission Order { get; set; }
    }
}
