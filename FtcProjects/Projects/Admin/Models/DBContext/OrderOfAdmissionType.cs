using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OrderOfAdmissionType
    {
        public OrderOfAdmissionType()
        {
            OrderOfAdmission = new HashSet<OrderOfAdmission>();
        }

        public int OrderOfAdmissionTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderOfAdmission> OrderOfAdmission { get; set; }
    }
}
