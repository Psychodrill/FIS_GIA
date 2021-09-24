using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AdmissionRules
    {
        public int RecordId { get; set; }
        public int InstitutionId { get; set; }
        public int Year { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] File { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}
