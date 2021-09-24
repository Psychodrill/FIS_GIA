using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OrderOfAdmissionStatus
    {
        public OrderOfAdmissionStatus()
        {
            OrderOfAdmission = new HashSet<OrderOfAdmission>();
        }

        public int OrderOfAdmissionStatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderOfAdmission> OrderOfAdmission { get; set; }
    }
}
