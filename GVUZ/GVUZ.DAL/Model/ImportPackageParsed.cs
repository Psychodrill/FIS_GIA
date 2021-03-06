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
    
    public partial class ImportPackageParsed
    {
        public int Id { get; set; }
        public Nullable<int> PackageID { get; set; }
        public Nullable<int> InstitutionID { get; set; }
        public Nullable<System.DateTime> PackageCreatedDate { get; set; }
        public Nullable<System.DateTime> PackageModifiedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ApplicationNumber { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<int> DirectionID { get; set; }
        public Nullable<int> EducationFormID { get; set; }
        public Nullable<int> FinanceSourceID { get; set; }
        public Nullable<int> EducationLevelID { get; set; }
        public Nullable<int> Stage { get; set; }
        public bool IsBeneficiary { get; set; }
        public bool IsForeigner { get; set; }
        public Nullable<int> Status { get; set; }
        public string Comment { get; set; }
        public Nullable<int> ApplicationID { get; set; }
        public int TrashTypeID { get; set; }
    }
}
