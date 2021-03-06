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
    
    public partial class AdmissionData
    {
        public int AdmissionStructureID { get; set; }
        public int AdmissionItemID { get; set; }
        public int InstitutionID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public short Depth { get; set; }
        public string Lineage { get; set; }
        public short ItemTypeID { get; set; }
        public short ItemLevel { get; set; }
        public Nullable<int> PlaceCount { get; set; }
        public Nullable<int> TotalDirectionPlaceCount { get; set; }
        public string Name { get; set; }
        public Nullable<bool> HasMilitaryDepartment { get; set; }
        public Nullable<bool> HasPreparatoryCourses { get; set; }
        public Nullable<bool> HasOlympics { get; set; }
        public Nullable<short> EducationLevelID { get; set; }
        public Nullable<int> DirectionID { get; set; }
        public Nullable<short> StudyID { get; set; }
        public Nullable<short> AdmissionTypeID { get; set; }
        public bool IsLeaf { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
