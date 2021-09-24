using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationForcedAdmissionReason
    {
        public ApplicationForcedAdmissionReason()
        {
            Application = new HashSet<Application>();
        }

        public int ApplicationForcedAdmissionReasonsId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Application> Application { get; set; }
    }
}
