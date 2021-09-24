using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationSelectedCompetitiveGroupItem
    {
        public int ItemId { get; set; }
        public int ApplicationId { get; set; }
        public int CompetitiveGroupItemId { get; set; }

        public virtual Application Application { get; set; }
        public virtual CompetitiveGroupItem CompetitiveGroupItem { get; set; }
    }
}
