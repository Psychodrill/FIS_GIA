using System;

namespace GVUZ.Util.Services.Parser
{
    public class ImportPackageDto
    {
        public string PackageData { get; set; }
        public int InstitutionId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastDateChanged { get; set; }
        public int PackageId { get; set; }
    }
}