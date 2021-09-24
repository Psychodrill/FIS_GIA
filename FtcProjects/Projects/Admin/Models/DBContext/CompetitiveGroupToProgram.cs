using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CompetitiveGroupToProgram
    {
        public int CompetitiveGroupProgramId { get; set; }
        public int InstitutionProgramId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual InstitutionProgram InstitutionProgram { get; set; }
    }
}
