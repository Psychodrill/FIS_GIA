using System;

namespace GVUZ.AppExport.Services
{
    public class ApplicationExportRequest
    {
        public Guid RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public ApplicationExportRequestStatus RequestStatus { get; set; }
        public long InstitutionId { get; set; }
        public int YearStart { get; set; }
    }
}