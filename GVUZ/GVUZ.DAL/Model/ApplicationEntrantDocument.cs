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
    
    public partial class ApplicationEntrantDocument
    {
        public int ID { get; set; }
        public int ApplicationID { get; set; }
        public int EntrantDocumentID { get; set; }
        public Nullable<int> AttachedDocumentType { get; set; }
        public string UID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> OriginalReceivedDate { get; set; }
    
        public virtual Application Application { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
    }
}