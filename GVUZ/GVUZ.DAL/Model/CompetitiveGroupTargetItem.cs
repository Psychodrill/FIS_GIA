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
    
    public partial class CompetitiveGroupTargetItem
    {
        public int CompetitiveGroupTargetItemID { get; set; }
        public int CompetitiveGroupTargetID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> NumberTargetO { get; set; }
        public Nullable<int> NumberTargetOZ { get; set; }
        public Nullable<int> NumberTargetZ { get; set; }
        public int CompetitiveGroupID { get; set; }
    
        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual CompetitiveGroupTarget CompetitiveGroupTarget { get; set; }
    }
}
