using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class InstitutionProgram
    {
        public InstitutionProgram()
        {
            CompetitiveGroupToProgram = new HashSet<CompetitiveGroupToProgram>();
        }

        public int InstitutionProgramId { get; set; }
        public int InstitutionId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Institution Institution { get; set; }
        public virtual ICollection<CompetitiveGroupToProgram> CompetitiveGroupToProgram { get; set; }
    }
}
