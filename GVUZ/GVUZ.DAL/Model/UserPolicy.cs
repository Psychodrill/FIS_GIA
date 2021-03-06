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
    
    public partial class UserPolicy
    {
        public System.Guid UserID { get; set; }
        public Nullable<int> InstitutionID { get; set; }
        public bool IsMainAdmin { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string Comment { get; set; }
        public int AvailableEgeCheckCount { get; set; }
        public string PinCode { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public bool IsReadOnly { get; set; }
        public int Subrole { get; set; }
        public Nullable<int> FilialID { get; set; }
        public Nullable<bool> IsDeactivated { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual Institution Institution { get; set; }
    }
}
