namespace GVUZ.Web.Infrastructure
{
    public interface ISortable
    {
        string SortKey { get; }
        bool SortDescending { get; }
    }
}