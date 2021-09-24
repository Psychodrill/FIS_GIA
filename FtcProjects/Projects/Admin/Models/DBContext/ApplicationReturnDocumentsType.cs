using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationReturnDocumentsType
    {
        public ApplicationReturnDocumentsType()
        {
            Application = new HashSet<Application>();
        }

        public int ApplicationReturnDocumentsTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Application> Application { get; set; }
    }
}
