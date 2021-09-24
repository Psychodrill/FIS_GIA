using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationExtraDefinition
    {
        public ApplicationExtraDefinition()
        {
            ApplicationExtra = new HashSet<ApplicationExtra>();
        }

        public int ApplicationExtraDefinitionId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<ApplicationExtra> ApplicationExtra { get; set; }
    }
}
