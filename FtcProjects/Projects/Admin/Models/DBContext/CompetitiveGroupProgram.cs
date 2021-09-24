using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CompetitiveGroupProgram
    {
        public long ProgramId { get; set; }
        public string Uid { get; set; }
        public int CompetitiveGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
    }
}
