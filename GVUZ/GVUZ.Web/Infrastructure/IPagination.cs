namespace GVUZ.Web.Infrastructure
{
    public interface IPagination
    {
        int TotalRecords { get; set; }
        int FirstRecordOffset { get; }
        int LastRecordOffset { get; }
    }
}