using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class RviPersonIdOld
    {
        public int? RvipersonId { get; set; }
        public string RvinormSurname { get; set; }
        public string RvinormName { get; set; }
        public string RvinormSecondName { get; set; }
        public DateTime? RvibirthDay { get; set; }
        public int? EentrantId { get; set; }
        public int? EidentityDocumentId { get; set; }
        public string ElastName { get; set; }
        public string EfirstName { get; set; }
        public string EmiddleName { get; set; }
        public string EddocumentSeries { get; set; }
        public string EddocumentNumber { get; set; }
        public int? EdiidentityDocumentTypeId { get; set; }
        public DateTime? EdibirthDate { get; set; }
        public Guid? PparticipantId { get; set; }
        public string Psurname { get; set; }
        public string Pname { get; set; }
        public string PsecondName { get; set; }
        public string PdocumentSeries { get; set; }
        public string PdocumentNumber { get; set; }
        public int? PdocumentTypeCode { get; set; }
        public DateTime? PbirthDay { get; set; }
    }
}
