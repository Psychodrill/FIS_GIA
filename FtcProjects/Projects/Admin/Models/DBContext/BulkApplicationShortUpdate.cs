using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BulkApplicationShortUpdate
    {
        public int? Id { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string CustomInformation { get; set; }
        public int? CompetitiveGroupId { get; set; }
        public DateTime? IsAgreedDate { get; set; }
        public int? EntrantDocumentId { get; set; }
        public string EntrantDocumentUid { get; set; }
        public DateTime? OriginalReceivedDate { get; set; }
        public int? StatusId { get; set; }
        public DateTime? IsDisagreedDate { get; set; }
    }
}
