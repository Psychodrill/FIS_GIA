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
    
    public partial class Translation_RVIDT_IdentityDT
    {
        public int ID { get; set; }
        public int DocumentTypeCode { get; set; }
        public int IdentityDocumentTypeID { get; set; }
    
        public virtual IdentityDocumentType IdentityDocumentType { get; set; }
        public virtual RVIDocumentTypes RVIDocumentTypes { get; set; }
    }
}
