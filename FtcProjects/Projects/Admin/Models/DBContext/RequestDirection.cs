using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class RequestDirection
    {
        public RequestDirection()
        {
            RequestComments = new HashSet<RequestComments>();
        }

        public int DirectionId { get; set; }
        public int RequestId { get; set; }
        public string Activity { get; set; }
        public string Action { get; set; }
        public int AdmissionItemType { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ChangeDate { get; set; }

        public virtual Institution Request { get; set; }
        public virtual ICollection<RequestComments> RequestComments { get; set; }
    }
}
