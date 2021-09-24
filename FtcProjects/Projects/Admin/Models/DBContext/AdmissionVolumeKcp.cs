using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AdmissionVolumeKcp
    {
        public double? AdmissionVolumeId { get; set; }
        public double? InstitutionId { get; set; }
        public double? AdmissionItemTypeId { get; set; }
        public string DirectionId { get; set; }
        public double? NumberBudgetO { get; set; }
        public double? NumberBudgetOz { get; set; }
        public double? NumberBudgetZ { get; set; }
        public double? Year { get; set; }
    }
}
