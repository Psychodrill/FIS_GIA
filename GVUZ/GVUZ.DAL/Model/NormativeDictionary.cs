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
    
    public partial class NormativeDictionary
    {
        public int DictionaryID { get; set; }
        public string Name { get; set; }
        public int VersionID { get; set; }
        public Nullable<System.DateTime> ActivationDate { get; set; }
        public byte VersionStateID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual NormativeVersionState NormativeVersionState { get; set; }
    }
}