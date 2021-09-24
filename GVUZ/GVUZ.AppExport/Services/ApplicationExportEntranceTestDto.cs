namespace GVUZ.AppExport.Services
{
    public class ApplicationExportEntranceTestDto
    {
        public long EntranceTestResultId { get; set; }
        public decimal ResultValue { get; set; }
        public long ResultSourceTypeId { get; set; }
        public long EntranceTestTypeId { get; set; }
    }
}