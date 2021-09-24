using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BlkApplication
    {
        public string EntrantUid { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ApplicationNumber { get; set; }
        public bool? NeedHostel { get; set; }
        public int StatusId { get; set; }
        public DateTime? LastDenyDate { get; set; }
        public string StatusDecision { get; set; }
        public bool IsRequiresBudgetO { get; set; }
        public bool IsRequiresBudgetOz { get; set; }
        public bool IsRequiresBudgetZ { get; set; }
        public bool IsRequiresPaidO { get; set; }
        public bool IsRequiresPaidOz { get; set; }
        public bool IsRequiresPaidZ { get; set; }
        public bool IsRequiresTargetO { get; set; }
        public bool IsRequiresTargetOz { get; set; }
        public bool IsRequiresTargetZ { get; set; }
        public bool? OriginalDocumentsReceived { get; set; }
        public DateTime? OriginalDocumentsReceivedDate { get; set; }
        public int? OrderOfAdmissionId { get; set; }
        public int? Priority { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Uid { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}
