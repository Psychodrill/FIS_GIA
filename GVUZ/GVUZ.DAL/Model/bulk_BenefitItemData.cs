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
    
    public partial class bulk_BenefitItemData
    {
        public System.Guid GUID { get; set; }
        public System.Guid ParentID { get; set; }
        public int InstitutionID { get; set; }
        public int ImportPackageID { get; set; }
        public Nullable<int> OlympicTypeID { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public Nullable<int> EgeMinValue { get; set; }
    }
}
