using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkApplication
    {
        public int? Id { get; set; }
        public Guid Guid { get; set; }
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }
        public string Uid { get; set; }
        public string EntrantUid { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool? NeedHostel { get; set; }
        public int StatusId { get; set; }
        public string StatusComment { get; set; }
        public bool IsRequiresBudgetO { get; set; }
        public bool IsRequiresBudgetOz { get; set; }
        public bool IsRequiresBudgetZ { get; set; }
        public bool IsRequiresPaidO { get; set; }
        public bool IsRequiresPaidOz { get; set; }
        public bool IsRequiresPaidZ { get; set; }
        public bool IsRequiresTargetO { get; set; }
        public bool IsRequiresTargetOz { get; set; }
        public bool IsRequiresTargetZ { get; set; }
        public int? ReturnDocumentsTypeId { get; set; }
        public DateTime? ReturnDocumentsDate { get; set; }
    }
}
