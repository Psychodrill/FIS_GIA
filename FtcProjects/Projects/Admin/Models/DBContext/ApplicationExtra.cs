using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationExtra
    {
        public int ApplicationExtraId { get; set; }
        public string Value { get; set; }
        public int ApplicationId { get; set; }
        public int ApplicationExtraDefinitionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Application Application { get; set; }
        public virtual ApplicationExtraDefinition ApplicationExtraDefinition { get; set; }
    }
}
