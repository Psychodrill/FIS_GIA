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
    
    public partial class ApplicationEntranceTestDocument
    {
        public int ID { get; set; }
        public int ApplicationID { get; set; }
        public Nullable<int> SubjectID { get; set; }
        public Nullable<int> EntrantDocumentID { get; set; }
        public Nullable<short> EntranceTestTypeID { get; set; }
        public Nullable<int> SourceID { get; set; }
        public Nullable<decimal> ResultValue { get; set; }
        public Nullable<short> BenefitID { get; set; }
        public Nullable<int> EntranceTestItemID { get; set; }
        public string UID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string InstitutionDocumentNumber { get; set; }
        public Nullable<System.DateTime> InstitutionDocumentDate { get; set; }
        public Nullable<int> InstitutionDocumentTypeID { get; set; }
        public Nullable<int> CompetitiveGroupID { get; set; }
        public bool HasEge { get; set; }
        public Nullable<decimal> EgeResultValue { get; set; }
        public Nullable<int> AppealStatusID { get; set; }
        public Nullable<bool> UsedInOrder { get; set; }
    
        public virtual AppealStatus AppealStatus { get; set; }
        public virtual Application Application { get; set; }
        public virtual Benefit Benefit { get; set; }
        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual EntranceTestItemC EntranceTestItemC { get; set; }
        public virtual EntranceTestResultSource EntranceTestResultSource { get; set; }
        public virtual EntranceTestType EntranceTestType { get; set; }
        public virtual EntrantDocument EntrantDocument { get; set; }
        public virtual InstitutionDocumentType InstitutionDocumentType { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
