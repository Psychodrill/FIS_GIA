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
    
    public partial class ApplicationCompetitiveGroupItem
    {
        public int id { get; set; }
        public int ApplicationId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public int CompetitiveGroupItemId { get; set; }
        public int EducationFormId { get; set; }
        public int EducationSourceId { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> CompetitiveGroupTargetId { get; set; }
    
        public virtual Application Application { get; set; }
        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual CompetitiveGroupItem CompetitiveGroupItem { get; set; }
    }
}
