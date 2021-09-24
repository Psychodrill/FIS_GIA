using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ParentDirectionPlanKcp
    {
        public int? InstitutionId { get; set; }
        public int? EducationLevelId { get; set; }
        public int? ParentDirectionId { get; set; }
        public int? NumberBudgetO { get; set; }
        public int? NumberBudgetZ { get; set; }
        public int? NumberBudgetOz { get; set; }
    }
}
