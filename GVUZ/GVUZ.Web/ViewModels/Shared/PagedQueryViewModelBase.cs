namespace GVUZ.Web.ViewModels.Shared
{
    public class PagedQueryViewModelBase
    {
        public PagerViewModel Pager { get; set; }
        public SortViewModel Sort { get; private set; }

        public PagedQueryViewModelBase(string defaultSortKey, bool sortDescending = false)
        {
            Sort = new SortViewModel(defaultSortKey, IsValidSortKey) { SortDescending = sortDescending };
        }

        protected virtual bool IsValidSortKey(string value)
        {
            return false;
        }
    }
}