using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GetCompetitiveGroupProgram
    {
        public int CompetitiveGroupProgramId { get; set; }
        public int InstitutionProgramId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public int InstitutionId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
