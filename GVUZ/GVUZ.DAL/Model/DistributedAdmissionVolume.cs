//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GVUZ.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class DistributedAdmissionVolume
    {
        public int DistributedAdmissionVolumeID { get; set; }
        public int AdmissionVolumeID { get; set; }
        public int IdLevelBudget { get; set; }
        public int NumberBudgetO { get; set; }
        public int NumberBudgetOZ { get; set; }
        public int NumberBudgetZ { get; set; }
        public int NumberQuotaO { get; set; }
        public int NumberQuotaOZ { get; set; }
        public int NumberQuotaZ { get; set; }
        public int NumberTargetO { get; set; }
        public int NumberTargetOZ { get; set; }
        public int NumberTargetZ { get; set; }
    
        public virtual AdmissionVolume AdmissionVolume { get; set; }
        public virtual LevelBudget LevelBudget { get; set; }
    }
}