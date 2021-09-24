using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BlkConsideredApplication
    {
        public DateTime RegistrationDate { get; set; }
        public string ApplicationNumber { get; set; }
        public int DirectionId { get; set; }
        public short? EducationLevelId { get; set; }
        public short EducationFormId { get; set; }
        public short FinanceSourceId { get; set; }
        public bool IsRequiresBudgetO { get; set; }
        public bool IsRequiresBudgetOz { get; set; }
        public bool IsRequiresBudgetZ { get; set; }
        public bool IsRequiresPaidO { get; set; }
        public bool IsRequiresPaidOz { get; set; }
        public bool IsRequiresPaidZ { get; set; }
        public bool IsRequiresTargetO { get; set; }
        public bool IsRequiresTargetOz { get; set; }
        public bool IsRequiresTargetZ { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
