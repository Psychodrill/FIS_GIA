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
    
    public partial class bulk_CampaignDate
    {
        public System.Guid Id { get; set; }
        public System.Guid ParentId { get; set; }
        public int Course { get; set; }
        public short EducationLevelID { get; set; }
        public short EducationFormID { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<System.DateTime> DateOrder { get; set; }
        public string UID { get; set; }
        public bool IsActive { get; set; }
        public int Stage { get; set; }
        public short EducationSourceID { get; set; }
        public int ImportPackageID { get; set; }
        public int InstitutionID { get; set; }
    }
}
