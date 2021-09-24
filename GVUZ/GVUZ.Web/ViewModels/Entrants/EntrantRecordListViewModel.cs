using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.Entrants
{
    public class EntrantRecordListViewModel : PagedListViewModelBase<EntrantRecordViewModel>
    {
        private EntrantRecordListFilterViewModel _filter;

        public EntrantRecordListFilterViewModel Filter
        {
            get { return _filter ?? (_filter = new EntrantRecordListFilterViewModel()); }
            set { _filter = value; }
        }

        public int TotalRecordsCount { get; set; }

        public EntrantRecordViewModel RecordInfo = null;
    }
}