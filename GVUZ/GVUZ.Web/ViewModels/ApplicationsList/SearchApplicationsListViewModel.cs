using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class SearchApplicationsListViewModel : PagedListViewModelBase<SearchApplicationsRecordViewModel>
    {
        private static readonly SearchApplicationsRecordViewModel MetadataInstance = new SearchApplicationsRecordViewModel();

        public SearchApplicationsRecordViewModel RecordInfo
        {
            get { return MetadataInstance; }
        }

        private SearchApplicationsFilterViewModel _filter;

        public SearchApplicationsFilterViewModel Filter
        {
            get { return _filter ?? (_filter = new SearchApplicationsFilterViewModel()); }
            set { _filter = value; }
        }

        public int TotalApplicationsCount { get; set; }
    }
}