using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationStatusType
    {
        public ApplicationStatusType()
        {
            Application = new HashSet<Application>();
        }

        public int StatusId { get; set; }
        public string Name { get; set; }
        public bool UseForAppLimitValidation { get; set; }
        public bool IsActiveApp { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Application> Application { get; set; }
    }
}
