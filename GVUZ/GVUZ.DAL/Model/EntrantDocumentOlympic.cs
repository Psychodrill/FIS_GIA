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
    
    public partial class EntrantDocumentOlympic
    {
        public int EntrantDocumentID { get; set; }
        public Nullable<int> DiplomaTypeID { get; set; }
        public Nullable<int> OlympicID { get; set; }
        public Nullable<int> ClassNumber { get; set; }
        public Nullable<int> OlympicProfileID { get; set; }
    
        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual OlympicProfile OlympicProfile { get; set; }
    }
}
